using System;
using AutoMapper;

namespace WebAPIService.MapperProfiles {
    public class ChildProfile : Profile {
        public ChildProfile () {
            CreateMap<DataAccess.DataBaseEntities.Child<Guid>, Contracts.Shared.Results.Child> ()
                .ForMember (dest => dest.Name, opt => opt.MapFrom (src => $"{src.FirstName} {src.LastName}"));
        }
    }
}