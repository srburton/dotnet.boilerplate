using SimpleInjector;
using SimpleInjector.Lifestyles;
using System;
using System.Linq;

namespace App.Infra.Bootstrap
{
    public class Ioc
    {
        static Container _container;

        static Ioc()
        {
            InitializeContainer();
        }

        static void InitializeContainer()
        {
            _container = new Container();
            _container.Options.AllowOverridingRegistrations = true;
            _container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();
        }

        public static Scope BeginAsyncScope()
            => AsyncScopedLifestyle.BeginScope(_container);

        public static void RegisterSingleton<TInt, TImp>()
            where TImp : class, TInt
            where TInt : class
            => _container.RegisterSingleton<TInt, TImp>();

        public static void RegisterSingleton<TImp>()
            where TImp : class
            => _container.RegisterSingleton<TImp>();

        public static void RegisterScoped<TInt, TImp>()
            where TImp : class, TInt
            where TInt : class
            => _container.Register<TInt, TImp>(Lifestyle.Scoped);

        public static void RegisterScopedCollection<TInt>(params Type[] types)
            where TInt : class
        {
            _container.Collection.Register(typeof(TInt), types);

            _container.Register(() => _container.GetAllInstances<TInt>().ToArray(), Lifestyle.Scoped);
        }

        public static void RegisterSingletonCollection<TInt>(params Type[] types)
            where TInt : class
        {
            _container.Collection.Register(typeof(TInt), types);

            _container.Register(() => _container.GetAllInstances<TInt>().ToArray(), Lifestyle.Singleton);
        }

        public static void RegisterTransient<TInt, TImp>()
            where TImp : class, TInt
            where TInt : class
            => _container.Register<TInt, TImp>(Lifestyle.Transient);

        public static void RegisterTransient<TImp>()
          where TImp : class
          => _container.Register<TImp>(Lifestyle.Transient);

        public static TService Get<TService>()
            where TService : class
            => _container.GetInstance<TService>();

        public static bool AreRegistrationsValid()
        {
            try
            {
                _container.Verify();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void ResolveWithInstance<TInt>(TInt pushPlataformStrategy)
            where TInt : class
        {
            _container.RegisterInstance(typeof(TInt), pushPlataformStrategy);
        }

        public static void ResolveManyWithInstance<TInt>(TInt pushPlataformStrategy)
            where TInt : class
            => _container.Register(typeof(TInt[]), () => new[] { pushPlataformStrategy }, Lifestyle.Scoped);

        public static void Reset()
            => InitializeContainer();

        public static Container RecoverContainer()
            => _container;
    }
}
