using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using Unity.Injection;

using Karmr.Common.Contracts;
using Karmr.Common.Infrastructure;
using Karmr.Domain.Commands;
using Karmr.Domain.Queries;
using Karmr.Persistence;

namespace Karmr.WebUI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // infrastructure
            container.RegisterType<IClock, SystemClock>();
            // repositories
            var connectionString = "Server=.;Database=Karmr;User Id=Karmr;Password=Karmr;";
            container.RegisterType<IEventRepository, SqlEventRepository>(new InjectionConstructor(new object[] { connectionString }));
            container.RegisterType<IDenormalizerRepository, DenormalizerRepository>(new InjectionConstructor(new object[] { connectionString }));
            container.RegisterType<IQueryRepository, QueryRepository>(new InjectionConstructor(new object[] { connectionString }));
            // misc
            container.RegisterFactory(
                typeof(ICommandHandler),
                (unityContainer, type, name) =>
                    CommandHandlerFactory.Create(unityContainer.Resolve<IClock>(), unityContainer.Resolve<IEventRepository>(), unityContainer.Resolve<IDenormalizerRepository>()));
            container.RegisterType<ListingQueries>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}