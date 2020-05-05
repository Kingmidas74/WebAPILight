using System;
using Domain;
using MediatR;

namespace BusinessServices.Modules.ParentModule
{
    public class CreateParentCommand:IRequest<Parent>
    {
        public Guid Id {get; set;}
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
    }
}