using System;
using AutoMapper;

namespace WebAPIService.MapperProfiles {
    public class ChildProfile : Profile {
        public ChildProfile () {
            CreateMap<DataAccess.DataBaseEntities.Child<Guid>, BusinessServices.Models.Child<Guid>> ().ReverseMap();
        }
    }
}