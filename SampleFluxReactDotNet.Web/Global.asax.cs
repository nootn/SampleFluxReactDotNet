using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Builder;
using Autofac.Integration.Mvc;
using AutofacContrib.DynamicProxy;
using DotNetAppStarterKit.Core.Command;
using DotNetAppStarterKit.Core.Event;
using DotNetAppStarterKit.Core.Query;
using DotNetAppStarterKit.Web.Caching;
using DotNetAppStarterKit.Web.Logging;
using DotNetAppStarterKit.Web.Security;
using EventStore.ClientAPI;
using SampleFluxReactDotNet.Core.EventStore;
using SampleFluxReactDotNet.Core.EventStore.Interface;
using SampleFluxReactDotNet.Web.Application.Interceptors;

namespace SampleFluxReactDotNet.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ConfigureIoc();
        }

        private static void ConfigureIoc()
        {
            var builder = new ContainerBuilder();
            var webAssembly = typeof (MvcApplication).Assembly;
            var coreAssembly = typeof (ManipulateEvent).Assembly;
            //Web components
            builder.RegisterControllers(webAssembly);
            builder.RegisterFilterProvider();
            builder.RegisterModule(new AutofacWebTypesModule());

            //Components defined within this application
            builder.RegisterType<LoggerInterceptor>().AsSelf();
            builder.RegisterType<EventStoreProxy>().AsImplementedInterfaces().SingleInstance();
            //.EnableInterfaceInterceptors().InterceptedBy(typeof (LoggerInterceptor));

            //DotNetAppStarterKit components
            builder.RegisterType<User>().AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterGeneric(typeof (WebCacheProvider<>)).AsImplementedInterfaces().InstancePerRequest();
            builder.RegisterGeneric(typeof (TraceWithElmahErrorsAndCallerInfoLog<>))
                .AsImplementedInterfaces()
                .InstancePerRequest();
            RegisterGenericTypes(builder, coreAssembly, typeof (ICommand<,>), true)
                .ForEach(
                    _ =>
                        _.InstancePerRequest()
                            .EnableInterfaceInterceptors()
                            .InterceptedBy(typeof (LoggerInterceptor)));
            RegisterGenericTypes(builder, coreAssembly, typeof (ICommand<>), true)
                .ForEach(
                    _ =>
                        _.InstancePerRequest()
                            .EnableInterfaceInterceptors()
                            .InterceptedBy(typeof (LoggerInterceptor)));
            RegisterGenericTypes(builder, coreAssembly, typeof (IQuery<,>), true)
                .ForEach(
                    _ =>
                        _.InstancePerRequest()
                            .EnableInterfaceInterceptors()
                            .InterceptedBy(typeof (LoggerInterceptor)));
            RegisterGenericTypes(builder, coreAssembly, typeof (IQuery<>), true)
                .ForEach(
                    _ =>
                        _.InstancePerRequest()
                            .EnableInterfaceInterceptors()
                            .InterceptedBy(typeof (LoggerInterceptor)));
            RegisterGenericTypes(builder, coreAssembly, typeof (ICachedQuery<,>), true)
                .ForEach(
                    _ =>
                        _.InstancePerRequest()
                            .EnableInterfaceInterceptors()
                            .InterceptedBy(typeof (LoggerInterceptor)));
            RegisterGenericTypes(builder, coreAssembly, typeof (ICachedQuery<>), true)
                .ForEach(
                    _ =>
                        _.InstancePerRequest()
                            .EnableInterfaceInterceptors()
                            .InterceptedBy(typeof (LoggerInterceptor)));
            builder.RegisterGeneric(typeof (EventPublisher<>)).AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterGeneric(typeof (EventSubscribersProvider<>))
                .AsImplementedInterfaces()
                .InstancePerRequest();
            RegisterGenericTypes(builder, coreAssembly, typeof (IEventSubscriber<>), true)
                .ForEach(
                    _ =>
                        _.InstancePerRequest()
                            .EnableInterfaceInterceptors()
                            .InterceptedBy(typeof (LoggerInterceptor)));

            //Build and set resolver
            try
            {
                var container = builder.Build();
                DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            }
            catch (Exception ex)
            {
                if (ex is ReflectionTypeLoadException)
                {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                    throw new AggregateException(typeLoadException.Message, loaderExceptions);
                }
                throw;
            }

            //Make sure the event store connection works early
            var prox = DependencyResolver.Current.GetService<IEventStoreProxy>();
        }

        private static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();
            if (interfaceTypes.Where(it => it.IsGenericType).Any(it => it.GetGenericTypeDefinition() == genericType))
                return true;
            var baseType = givenType.BaseType;
            if (baseType == null) return false;
            return baseType.IsGenericType &&
                   baseType.GetGenericTypeDefinition() == genericType ||
                   IsAssignableToGenericType(baseType, genericType);
        }

        private static List<IRegistrationBuilder<object, object, object>> RegisterGenericTypes(ContainerBuilder builder,
            Assembly parentAssembly,
            Type
                genericRepositoryType,
            bool asInterfaces)
        {
            var types = parentAssembly.GetExportedTypes()
                .Where(t => !t.IsInterface && !t.IsAbstract)
                .Where(t => IsAssignableToGenericType(t, genericRepositoryType))
                .ToArray();
            var res = new List<IRegistrationBuilder<object, object, object>>();
            foreach (var type in types)
            {
                if (type.IsGenericType)
                    res.Add(asInterfaces
                        ? builder.RegisterGeneric(type).AsImplementedInterfaces()
                        : builder.RegisterGeneric(type).AsSelf());
                else
                    res.Add(asInterfaces
                        ? builder.RegisterType(type).AsImplementedInterfaces()
                        : builder.RegisterType(type).AsSelf());
            }
            return res;
        }
    }
}