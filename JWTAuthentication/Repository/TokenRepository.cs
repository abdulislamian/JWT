using JWTAuthentication.Data;
using JWTAuthentication.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;
        private readonly JWTDbContext dbContext;

        public TokenRepository(IConfiguration configuration,JWTDbContext _dbContext)
        {
            _configuration = configuration;
            dbContext = _dbContext;
        }
        public string CreateJWTToken(ApplicationUser user, List<string> roles)
        {
            //Create SomeClaims 
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }

        public bool IsTokenRevoked(string token)
        {
            bool isAvailable = false;
            isAvailable =  dbContext.tokenStores.Any(ts => ts.Token == token && ts.isValid ==true);
            return isAvailable;
        }


        public async Task<TokenStore> StoreToken(string id, string jWTToken)
        {
            var userToken = dbContext.tokenStores.FirstOrDefault(x => x.UserId == id);
            if (userToken != null)
            {
                userToken.Token = jWTToken;
                await dbContext.SaveChangesAsync();
                return userToken;
            }
            else
            {
                var NewuserToken = new TokenStore
                {
                    Token = jWTToken,
                    UserId = id,
                    isValid = true
                };
                await dbContext.tokenStores.AddAsync(NewuserToken);
                await dbContext.SaveChangesAsync();
                return NewuserToken;
            }
        }
    }
}
