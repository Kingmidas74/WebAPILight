using System;
using System.Collections.Generic;
using BusinessServices.Base;
using BusinessServices.Interfaces;
using Contracts.Shared.Interfaces;
using DataAccess.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Practices.ServiceLocation;


namespace BusinessServices.Extensions {

    public static class SCE
    {
        public static IServiceProvider SC;
    }
    
    public static class BusinessServiceExtensions {
        public static IList<TEntity> Find<TEntity, TParameter> (this IEntityService<TEntity> service, TParameter parameter)
        where TParameter : ICommandParameter
        where TEntity : class, IEntity, new () {
            return service.Find (GetCommand<TEntity, TParameter> (parameter));
        }

        public static TEntity FindOne<TEntity, TParameter> (this IEntityService<TEntity> service, TParameter parameter)
        where TParameter : ICommandParameter
        where TEntity : class, IEntity, new () {
            return service.FindOne (GetCommand<TEntity, TParameter> (parameter));
        }

        public static TKey Execute<TEntity, TParameter, TKey> (this BusinessEntityService<TEntity, TKey> service, TParameter parameter)
        where TParameter : ICommandParameter
        where TEntity : class, IBusinessEntity<TKey>, new ()
        where TKey : IComparable<TKey>, IEquatable<TKey> {
            return service.Execute (GetCommand<TEntity, TParameter> (parameter));
        }

        public static TKey FindResponse<TEntity, TParameter, TKey> (this ResponseEntityService<TEntity, TKey> service, TParameter parameter)
        where TParameter : ICommandParameter
        where TEntity : class, IResponse<TKey>, new () {
            return service.GetResponse (GetCommand<TEntity, TParameter> (parameter));
        }

        private static IEntityCommand<TEntity> GetCommand<TEntity, TParameter> (TParameter parameter)
        where TParameter : ICommandParameter
        where TEntity : class, new () {            
            var command = SCE.SC.GetService<IEntityDataCommand<TEntity, TParameter>>();
            command.Parameter = parameter;
            return command;
        }
    }
}