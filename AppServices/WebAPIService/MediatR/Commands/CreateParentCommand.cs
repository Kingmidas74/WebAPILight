using System;
using BusinessServices.Models;
using MediatR;

namespace WebAPIService.MediatR
{
    public class CreateParentCommand<T>:IRequest<Parent<T>>
    {
        public T Id {get; set;}
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
    }
}