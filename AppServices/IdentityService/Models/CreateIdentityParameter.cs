using System;
namespace IdentityService.Models
{
    public class CreateIdentityParameter
    {
        public Guid Id {get;set;}
        public string Password {get;set;}
        public string Email {get;set;}
        public string Phone {get;set;}
    }
}