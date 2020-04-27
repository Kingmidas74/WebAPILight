using System;
using System.Collections.Generic;
using System.Linq;
using BusinessServices.Base;
using DTO = Contracts.Shared.Results;
using DataAccess;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace BusinessServices.Services
{
    public class ChildService : BaseEntityService<DTO.Child> {

        public ChildService(APIContext<Guid> DbContext, IMapper Mapper):base(DbContext, Mapper)
        {

        }
        public override IEnumerable<DTO.Child> Find()
        {
            throw new NotImplementedException();
        }

        public override DTO.Child FindOne()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DTO.Child> GetAllByParent (Guid parentId) {
            return DbContext.Parents.SingleOrDefault(x=>x.Id.Equals(parentId))?.Children.ProjectTo<DTO.Child>(Mapper.ConfigurationProvider).AsEnumerable();
        }
    }
}