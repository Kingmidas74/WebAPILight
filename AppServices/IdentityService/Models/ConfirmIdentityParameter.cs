using System;
namespace IdentityService.Models
{
    public class ConfirmIdentityParameter
    {
        public Guid Id {get;set;}
        public string Code {get;set;}

        public string Redirect {get;set;}
    }
}