using Microsoft.AspNetCore.SignalR;

namespace JWTAuthentication.Models
{
    public class ForgetPasswordModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
