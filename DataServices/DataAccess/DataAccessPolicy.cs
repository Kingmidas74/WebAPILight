using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Contracts.Shared.Parameters;
using Contracts.Shared.Results;
using Dapper.FluentMap;
using DataAccess.Extensions;
using DataAccess.Extensions.StoredProcAttributes;
using DataAccess.Interfaces;
using DataAccess.Mapping;
using DataAccess.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess {
    public static class DataAccessPolicy {
        public static string ConnectionString = String.Empty;
        public static void Init (IServiceCollection container) {
            container.AddScoped<IModel<Parent>, ParentModel>();
            container.AddScoped<IModel<Child>, ChildModel> ();

            container.AddScoped(typeof (IEntityDataCommand<,>), typeof (ModelBaseCommand<,>));

            InitializeStoredProcs ();

            FluentMapper.Initialize (config => {

                Type generic = typeof (UserInputParameter);
                config.AddConvention<TypePrefixConvention> ()
                    .ForEntitiesInAssembly (generic.Assembly, nameof (Contracts.Shared.Results));

            });
        }

        public static void InitializeStoredProcs () {
            DbContext.Procedures = new ConcurrentDictionary<string, object> ();
            var modelsTypes = Assembly.GetExecutingAssembly ().DefinedTypes
                .Where (t => t.GetInterfaces ().Any (i => i.IsGenericType && i.GetGenericTypeDefinition () == typeof (IModel<>))).ToArray ();

            var properties = modelsTypes.SelectMany (model => model.DeclaredProperties
                .Where (property => property.PropertyType.GetGenericTypeDefinition () == typeof (StoredProcedure<>) &&
                    property.PropertyType.GetGenericTypeDefinition ().GetGenericArguments ().Length == 1)
                .Select (property => new { model, property })).ToArray ();

            var procs = properties
                .ToDictionary (
                    p => p.model.GetInterfaces ().First (i => i.IsGenericType && i.GetGenericTypeDefinition () == typeof (IModel<>))
                    .GetGenericArguments ().First ().Name + p.property.Name,
                    p => Assembly.GetExecutingAssembly ().CreateInstance (p.property.PropertyType.FullName,
                        ignoreCase : false,
                        bindingAttr : BindingFlags.Default,
                        binder : null,
                        args : new object[] {
                            p.property.GetCustomAttribute<ReturnTypesAttribute> ().ReturnType,
                                (DbContext) Activator.CreateInstance (p.model.BaseType),
                                p.property.GetCustomAttribute<NameAttribute> ().Name
                        },
                        culture : CultureInfo.CurrentCulture,
                        activationAttributes : null)).ToList ();
            procs.ForEach (p => DbContext.Procedures.GetOrAdd (p.Key, p.Value));
        }
    }
}