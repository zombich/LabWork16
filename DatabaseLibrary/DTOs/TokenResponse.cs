using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.DTOs
{
    public class TokenResponse(string token, string refreshToken)
    {
        public string Token { get; set; } = token;
        public string? RefreshToken { get; set; } = refreshToken;
    }
}
