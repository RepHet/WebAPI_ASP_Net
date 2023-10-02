using System.Collections.Generic;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using WebAPI_ASP_Net.Repositories;
using WebAPI_ASP_Net.Repositories.Containers;

namespace WebAPI_ASP_Net
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();


            container.RegisterType<ListContainer>(new ContainerControlledLifetimeManager());
            container.RegisterType<DictionaryContainer>(new ContainerControlledLifetimeManager());
            container.RegisterType<QueueContainer>(new ContainerControlledLifetimeManager());
            container.RegisterType<StackContainer>(new ContainerControlledLifetimeManager());

            container.RegisterType<ICollectionRepository<int>, ListRepository>();
            container.RegisterType<ICollectionRepository<KeyValuePair<int, int>>, DictionaryRepository>();
            container.RegisterType<ICollectionRepository<int>, QueueRepository>();
            container.RegisterType<ICollectionRepository<int>, StackRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}