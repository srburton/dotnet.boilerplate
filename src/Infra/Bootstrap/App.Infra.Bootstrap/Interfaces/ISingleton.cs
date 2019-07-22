namespace App.Infra.Bootstrap
{ 
    /// <summary>
    /// https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2
    /// Singleton lifetime services are created 
    /// the first time they're requested (or when ConfigureServices is run and an instance is specified with the service registration). 
    /// Every subsequent request uses the same instance. If the app requires singleton behavior, 
    /// allowing the service container to manage the service's lifetime is recommended. 
    /// Don't implement the singleton design pattern and provide user code to manage the object's lifetime in the class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISingleton<T>
    {
    }
}
