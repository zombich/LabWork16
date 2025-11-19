namespace DatabaseLibrary.DTOs
{
    public class LoginRequest(string login, string password)
    {
        public string Login { get; set; } = login;
        public string Password { get; set; } = password;
    }
}
