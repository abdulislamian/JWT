using JWTAuthentication.Data;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Identity;

namespace JWTAuthentication.Repository
{
    public interface ITokenRepository
    {
        string CreateJWTToken(ApplicationUser user, List<string> roles);
        bool IsTokenRevoked(string token);
        Task<TokenStore> StoreToken (string id, string jWTToken);
    }
}
