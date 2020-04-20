using System;
using System.Collections.Generic;
using BusinessServices.Base;
using BusinessServices.Extensions;
using Contracts.Shared.Results;
using Domain.Parameters;

namespace BusinessServices.Services
{
    public class ChildService : IdentifiedEntityService<Child> {
        public IEnumerable<Child> GetAllByParent (Guid sessionId, Guid parentId) {
            return this.Find (new GetAllByParentParameter {
                ParentId = parentId,
                UserId = sessionId
            });
        }
    }
}