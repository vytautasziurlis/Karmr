using Karmr.Common.Contracts;
using Karmr.Common.Infrastructure;
using Karmr.Domain.Commands;
using Karmr.Domain.Queries;
using Karmr.Persistence;
using Karmr.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Configuration;
using System.Web;
using Unity;
using Unity.Injection;

namespace Karmr.WebUI
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            // infrastructure
            container.RegisterType<IClock, SystemClock>();

            // asp.net identity
            container.RegisterType<ApplicationDbContext>();
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new InjectionConstructor(typeof(ApplicationDbContext)));
            container.RegisterType<ApplicationUserManager>();
            container.RegisterFactory(typeof(IAuthenticationManager),
                (unityContainer, type, name) => HttpContext.Current.GetOwinContext().Authentication);
            container.RegisterType<ApplicationSignInManager>();

            // repositories
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            container.RegisterType<IEventRepository, SqlEventRepository>(new InjectionConstructor(new object[] { connectionString }));
            container.RegisterType<IDenormalizerRepository, DenormalizerRepository>(new InjectionConstructor(new object[] { connectionString }));
            container.RegisterType<IQueryRepository, QueryRepository>(new InjectionConstructor(new object[] { connectionString }));
            // misc
            container.RegisterFactory(
                typeof(ICommandHandler),
                (unityContainer, type, name) =>
                    CommandHandlerFactory.Create(unityContainer.Resolve<IClock>(), unityContainer.Resolve<IEventRepository>(), unityContainer.Resolve<IDenormalizerRepository>()));
            container.RegisterType<ListingQueries>();
        }
    }
}