namespace DatabaseLibrary.DTOs
{
    public class TokenResponse(string token, string? refreshToken = null)
    {
        public string Token { get; set; } = token;
        public string? RefreshToken { get; set; } = refreshToken;
    }
}
