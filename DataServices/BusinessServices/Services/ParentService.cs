using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessServices.Base;
using BusinessServices.Extensions;
using Contracts.Shared.Enums;
using Contracts.Shared.Parameters;
using DTO = Contracts.Shared.Results;
using Contracts.Shared.Results;
using DataAccess;
using Domain.Parameters;

namespace BusinessServices.Services {
    public class ParentService : BaseEntityService<DTO.Parent> {

        public ParentService (APIContext<Guid> DbContext, IMapper Mapper) : base (DbContext, Mapper) {

        }

        public override IEnumerable<DTO.Parent> Find () {
            throw new NotImplementedException ();
        }

        public override DTO.Parent FindOne () {
            throw new NotImplementedException ();
        }

        public IEnumerable<DTO.Parent> GetAllPaged (int take = 0, int skip = 0, int sortIndex = 0, SortOrderTypes sortOrder = SortOrderTypes.ASC, FilterInputParameter filter = default) {
            var parents = this.DbContext.Parents;
            if (sortOrder == SortOrderTypes.ASC) {
                parents.OrderBy (x => x.CreatedDate);
            } else {
                parents.OrderByDescending (x => x.CreatedDate);
            }
            return parents.ProjectTo<DTO.Parent> (Mapper.ConfigurationProvider).AsEnumerable ();
        }

        public DTO.Parent GetById (Guid parentId) {
            return Mapper.Map<DTO.Parent> (this.DbContext.Parents.SingleOrDefault (x => x.Id.Equals (parentId)));
        }

        public void RemoveById (Guid parentId) {
            this.DbContext.Parents.Remove (this.DbContext.Parents.SingleOrDefault (x => x.Id.Equals (parentId)));
            this.DbContext.SaveChanges ();
        }
    }
}