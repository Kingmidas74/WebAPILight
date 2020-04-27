using System;
using AutoMapper;

namespace WebAPIService.MapperProfiles
{
    public class ParentProfile:Profile
    {
        public ParentProfile() {
            CreateMap<DataAccess.DataBaseEntities.Parent<Guid>, Contracts.Shared.Results.Parent>()
            .ForMember(dest=>dest.Name, opt=>opt.MapFrom(src=>$"{src.FirstName} {src.LastName}"));
        }
    }
}