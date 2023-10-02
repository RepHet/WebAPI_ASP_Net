using System.Collections.Generic;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using WebAPI_ASP_Net.Controllers;
using WebAPI_ASP_Net.Repositories;
using WebAPI_ASP_Net.Repositories.Containers.Dictionary;
using WebAPI_ASP_Net.Repositories.Containers.List;
using WebAPI_ASP_Net.Repositories.Containers.Queue;
using WebAPI_ASP_Net.Repositories.Containers.Stack;
using WebAPI_ASP_Net.Repositories.List;
using WebAPI_ASP_Net.Repositories.Queue;
using WebAPI_ASP_Net.Repositories.Stack;
using WebAPI_ASP_Net.Utils.Timer;

namespace WebAPI_ASP_Net
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Реєстрація контейнерів
            container.RegisterType(typeof(IListContainer<>), typeof(ListContainer<>), new ContainerControlledLifetimeManager());
            container.RegisterType(typeof(IDictionaryContainer<,>), typeof(DictionaryContainer<,>), new ContainerControlledLifetimeManager());
            container.RegisterType(typeof(IQueueContainer<>), typeof(QueueContainer<>), new ContainerControlledLifetimeManager());
            container.RegisterType(typeof(IStackContainer<>), typeof(StackContainer<>), new ContainerControlledLifetimeManager());

            // Реєстрація репозиторіїв залежностей
            container.RegisterType<IDictionaryRepository<int, int>, DictionaryRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IQueueRepository<int>, QueueRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IListRepository<int>, ListRepository>(new ContainerControlledLifetimeManager());
            container.RegisterType<IStackRepository<int>, StackRepository>(new ContainerControlledLifetimeManager());

            //container.RegisterType(typeof(IDictionaryRepository<,>), typeof(DictionaryRepository), new ContainerControlledLifetimeManager());
            //container.RegisterType(typeof(IQueueRepository<>), typeof(QueueRepository), new ContainerControlledLifetimeManager());
            //container.RegisterType(typeof(IListRepository<>), typeof(ListRepository), new ContainerControlledLifetimeManager());
            //container.RegisterType(typeof(IStackRepository<>), typeof(StackRepository), new ContainerControlledLifetimeManager());

            container.RegisterType<ITimer, PerformanceTimer>();

            // Реєстрація контролерів (не потрібно реєструвати)

            // Встановлення резольвера для залежностей
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
