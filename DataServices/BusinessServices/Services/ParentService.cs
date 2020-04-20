using System;
using System.Collections.Generic;
using System.Linq;
using BusinessServices.Base;
using BusinessServices.Extensions;
using Contracts.Shared.Enums;
using Contracts.Shared.Parameters;
using Contracts.Shared.Results;
using Domain.Parameters;

namespace BusinessServices.Services {
    public class ParentService : IdentifiedEntityService<Parent> {
        public ICollection<Parent> GetAllPaged (Guid sessionId, int? take = null, int skip = 0, int sortIndex = 0, SortOrderTypes sortOrder = SortOrderTypes.ASC, FilterInputParameter filter = default) {
            return this.Find (new GetAllPagedParameter { UserId = sessionId, Take = take, Skip = skip, SortIndex = sortIndex, SortOrder = (int) sortOrder, Filter = filter?.Value ?? String.Empty }).ToList ();
        }
    }
}