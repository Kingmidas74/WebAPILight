using System;
using System.Collections.Generic;
using BusinessServices.Extensions;
using Contracts.Shared.Interfaces;
using Domain.Parameters;

namespace BusinessServices.Base {
    public class IdentifiedEntityService<TEntity> : BusinessEntityService<TEntity, Guid>
        where TEntity : class, IBusinessEntity<Guid>, new () {
            public virtual IList<TEntity> CheckPermissions (Guid sessionId, Guid? id = null) {
                return this.Find (new CheckPermissionsParameter { UserId = sessionId, Id = id.Value });
            }

            public virtual int? Create (Guid sessionId, TEntity entity) {
                if (entity != null) {
                    CheckPermissions (sessionId, entity.Id);
                    return default;
                }
                return null;
            }

            public virtual int? Update (Guid sessionId, TEntity entity) {
                if (entity != null) {
                    CheckPermissions (sessionId, entity.Id);
                    return default;
                }
                return null;
            }

            public virtual TEntity FindOne (Guid sessionId, Guid id) {
                return this.FindOne (new GetOneParameter { UserId = sessionId, Id = id });
            }

            public virtual IList<TEntity> FindAll (Guid sessionId) {
                return this.Find (new GetAllParameter { UserId = sessionId });
            }
        }
}