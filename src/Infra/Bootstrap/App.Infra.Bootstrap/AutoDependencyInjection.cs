using SimpleInjector;
using Microsoft.AspNetCore.Hosting;
using System.Reflection;
using System.Collections.Generic;
using App.Infra.Bootstrap.Attributes;

namespace App.Infra.Bootstrap
{
    public static class AutoDependencyInjection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="webHost"></param>
        /// <returns></returns>
        public static IWebHostBuilder UseAutoDependencyInjection(this IWebHostBuilder webHost)
        {
            var container = Ioc.RecoverContainer();

            var assemblies = ProcessAssembly();

            container.Register(typeof(IScoped<>), assemblies, Lifestyle.Scoped);
            container.Register(typeof(ITransient<>), assemblies, Lifestyle.Transient);
            container.Register(typeof(ISingleton<>), assemblies, Lifestyle.Singleton);
            
            container.Register(typeof(IApplication<>), assemblies, Lifestyle.Transient);
            container.Register(typeof(IRepository<>), assemblies, Lifestyle.Transient);

            //AutoResolver 
            container.ResolveUnregisteredType += UnregisteredType;

            return webHost;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        internal static void UnregisteredType(object sender, UnregisteredTypeEventArgs e)
        {
            var container = Ioc.RecoverContainer();

            if (e.UnregisteredServiceType.IsGenericType)
            {
                var type = e.UnregisteredServiceType
                            .GetGenericTypeDefinition();

                if (type == typeof(IService<>) ||
                    type == typeof(IApplication<>) ||
                    type == typeof(IRepository<>))
                {                    
                    if (type.GetCustomAttribute<SingletonAttribute>() != null)
                        e.Register(Lifestyle.Singleton.CreateRegistration(e.UnregisteredServiceType.GenericTypeArguments[0], container));
                    else if (type.GetCustomAttribute<ScopedAttribute>() != null)
                        e.Register(Lifestyle.Scoped.CreateRegistration(e.UnregisteredServiceType.GenericTypeArguments[0], container));
                    else
                        e.Register(Lifestyle.Transient.CreateRegistration(e.UnregisteredServiceType.GenericTypeArguments[0], container));
                }
                else if (type == typeof(ITransient<>))
                    e.Register(Lifestyle.Transient.CreateRegistration(e.UnregisteredServiceType.GenericTypeArguments[0], container));
                else if (type == typeof(ISingleton<>))
                    e.Register(Lifestyle.Singleton.CreateRegistration(e.UnregisteredServiceType.GenericTypeArguments[0], container));
                else if (type == typeof(ISingleton<>))
                    e.Register(Lifestyle.Scoped.CreateRegistration(e.UnregisteredServiceType.GenericTypeArguments[0], container));
            }
        }
        /// <summary>
        /// Renders the current assembly in the solution.
        /// </summary>
        /// <returns></returns>
        internal static Assembly[] ProcessAssembly()
        {
            var mainAsm = Assembly.GetEntryAssembly();

            var assemblies = new List<Assembly>();
            assemblies.Add(mainAsm);

            foreach (var refAsmName in mainAsm.GetReferencedAssemblies())
                assemblies.Add(Assembly.Load(refAsmName));

            return assemblies.ToArray();
        }
    }
}
