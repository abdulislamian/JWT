using Microsoft.AspNetCore.Identity;

namespace JWTAuthentication.Validator
{
    public class PasswordValidator : IPasswordValidator<IdentityUser>
    {
        private readonly int _passwordExpirationInDays;

        public PasswordValidator(int passwordExpirationInDays)
        {
            _passwordExpirationInDays = passwordExpirationInDays;
        }

        public async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user, string password)
        {
            var errors = new List<IdentityError>();

            //if (user.PasswordChangeDate.HasValue)
            //{
            //    var passwordAge = DateTime.Now - user.PasswordChangeDate.Value;

            //    if (passwordAge.TotalDays > _passwordExpirationInDays)
            //    {
            //        errors.Add(new IdentityError
            //        {
            //            Code = "PasswordExpired",
            //            Description = "Your password has expired. Please reset your password."
            //        });
            //    }
            //}

            if (errors.Count > 0)
            {
                return IdentityResult.Failed(errors.ToArray());
            }

            return IdentityResult.Success;
        }
    }

}
