
using AutoMapper;
using Domain;

namespace BusinessServices.Modules.ParentModule
{
    public class ParentProfile: Profile
    {
        public ParentProfile() {
            CreateMap<CreateParentCommand, Parent> ();
        }
    }
}