using Microsoft.AspNetCore.Identity;

namespace JWTAuthentication.Data
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime PasswordExpirationDate { get; set; }
    }
    
}
