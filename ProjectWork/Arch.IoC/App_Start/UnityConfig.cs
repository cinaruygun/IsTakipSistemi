using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Arch.Data.UnitofWork;
using Arch.Data.GenericRepository;
using Arch.Core;
using Arch.Service.Interfaces;
using Arch.Service.Services;

namespace Arch.IoC
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        public static void RegisterTypes(IUnityContainer container)
        {
            container.BindInRequestScope<IUnitofWork, UnitofWork>();
            /// <summary>
            /// Schema : common
            /// </summary>
            container.BindInRequestScope<IGenericRepository<Lookup>, GenericRepository<Lookup>>();
            container.BindInRequestScope<IGenericRepository<LookupList>, GenericRepository<LookupList>>();
            container.BindInRequestScope<IGenericRepository<Parameters>, GenericRepository<Parameters>>();
            container.BindInRequestScope<IGenericRepository<Person>, GenericRepository<Person>>();
            container.BindInRequestScope<IGenericRepository<Unit>, GenericRepository<Unit>>();
            /// <summary>
            /// Schema : file
            /// </summary>
            container.BindInRequestScope<IGenericRepository<Media>, GenericRepository<Media>>();
            /// <summary>
            /// Schema : log
            /// </summary>
            container.BindInRequestScope<IGenericRepository<ExceptionLog>, GenericRepository<ExceptionLog>>();
            container.BindInRequestScope<IGenericRepository<RequestLog>, GenericRepository<RequestLog>>();
            container.BindInRequestScope<IGenericRepository<TempRequestLog>, GenericRepository<TempRequestLog>>();
            /// <summary>
            /// Schema : work
            /// </summary>
            container.BindInRequestScope<IGenericRepository<Comment>, GenericRepository<Comment>>();
            container.BindInRequestScope<IGenericRepository<Project>, GenericRepository<Project>>();
            container.BindInRequestScope<IGenericRepository<Task>, GenericRepository<Task>>();
            container.BindInRequestScope<IGenericRepository<TaskHistory>, GenericRepository<TaskHistory>>();
            container.BindInRequestScope<IGenericRepository<TaskMedia>, GenericRepository<TaskMedia>>();
            /// <summary>
            /// Services
            /// </summary>
            container.BindInRequestScope<IPersonService, PersonService>();
            container.BindInRequestScope<ILogService, LogService>();
            container.BindInRequestScope<IUtilityService, UtilityService>();
            container.BindInRequestScope<IMediaService, MediaService>();
            container.BindInRequestScope<IWorkService, WorkService>();
        }

        public static void BindInRequestScope<T1, T2>(this IUnityContainer container) where T2 : T1
        {
            container.RegisterType<T1, T2>(new HierarchicalLifetimeManager());
        }
    }
}