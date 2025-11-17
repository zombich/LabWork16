using DatabaseLibrary.Contexts;
using DatabaseLibrary.DTOs;
using DatabaseLibrary.Models;
using DatabaseLibrary.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatabaseLibrary.Service
{
    public class AuthService(CinemaUserDbContext context)
    {
        private readonly CinemaUserDbContext _context = context;

        public async Task<TokenResponse> GenerateTokenAsync(CinemaUser user)
        {
            int minutes = 15;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthOptions.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var role = await GetUserRoleByLoginAsync(user.Login);

            var claims = new Claim[]
            {
                new ("id", user.UserId.ToString()),
                new ("login", user.Login),
                new ("role", role.Name),
            };

            var token = new JwtSecurityToken(signingCredentials: credentials,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience);

            return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token), null);
        }

        public async Task<bool> RegistrateUserAsync(LoginRequest request)
        {
            string login = request.Login;
            string password = request.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return false;

            var selectedUser = await FindUserByLoginAsync(login);

            if (selectedUser is not null)
                return false;

            CinemaUser user = new()
            {
                Login = login,
                PasswordHash = HashPassword(password),
                Role = await GetRoleByNameAsync("Посетитель"),
            };

            try
            {
                await AddUserAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<TokenResponse?> AuthUserAsync(LoginRequest request)
        {
            var login = request.Login;
            var password = request.Password;

            var user = await FindUserByLoginAsync(login);
            if (user is null)
                return null;

            if (user.LockedUntil.HasValue)
                await UnlockUserAsync(user);

            if (user.LockedUntil.HasValue)
                return null;

            if (VerifyPassword(password, user.PasswordHash))
                return await GenerateTokenAsync(user);

            await IncreaseFailedLoginAttemptsAsync(user);
            return null;
        }

        private async Task IncreaseFailedLoginAttemptsAsync(CinemaUser user)
        {
            int attempts = 3;
            int minutes = 1;

            user.FailedLoginAttempts++;
            if (user.FailedLoginAttempts >= attempts)
            {
                user.FailedLoginAttempts = 0;
                user.LockedUntil = DateTime.Now.AddMinutes(minutes);
            }
            await _context.SaveChangesAsync();
        }

        private async Task UnlockUserAsync(CinemaUser user)
        {
            if (user.LockedUntil <= DateTime.Now)
            {
                user.LockedUntil = null;
                await _context.SaveChangesAsync();
            }
        }

        private string HashPassword(string password)
            => BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        private bool VerifyPassword(string password, string passwordHash)
            => BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);

        public async Task<CinemaUserRole?> GetUserRoleByLoginAsync(string login)
        {
            var user = await _context.CinemaUsers
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Login == login);
            return user?.Role;
        }

        private async Task AddUserAsync(CinemaUser user)
        {
            await _context.CinemaUsers.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<CinemaUser?> FindUserByLoginAsync(string login)
            => await _context.CinemaUsers
                .FirstOrDefaultAsync(u => u.Login == login);

        private async Task<CinemaUserRole?> GetRoleByNameAsync(string name)
            => await _context.CinemaUserRoles
                .FirstOrDefaultAsync(r => r.Name == name);
    }
}
