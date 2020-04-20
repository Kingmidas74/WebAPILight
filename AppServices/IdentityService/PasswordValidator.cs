using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Newtonsoft.Json;

namespace IdentityService
{    
    public class PasswordValidator : IExtensionGrantValidator
    {
        private readonly IUserRepository _userRepository;

        public PasswordValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository; 
        }
        public string GrantType => "custom";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var userPhone = context.Request.Raw.Get("phone");
            var userPassword = context.Request.Raw.Get("password");

            if (string.IsNullOrEmpty(userPhone) || string.IsNullOrEmpty(userPassword))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }
            
            

            var result = await _userRepository.FindByPhoneAndPassword(userPhone,userPassword);
            if(result==null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            context.Result = new GrantValidationResult(userPhone, GrantType, new List<Claim> {
                new Claim("userId",result.Id.ToString()),
                new Claim("userEmail",result.Email),
            });
            return;
        }
    }
}