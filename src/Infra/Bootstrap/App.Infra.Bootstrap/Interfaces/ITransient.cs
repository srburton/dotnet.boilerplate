namespace App.Infra.Bootstrap
{
    /// <summary>
    /// https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-2.2
    /// Transient lifetime services are created each time they're requested from the service container. This lifetime works best for lightweight, stateless services.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITransient<T>
    {

    }
}
