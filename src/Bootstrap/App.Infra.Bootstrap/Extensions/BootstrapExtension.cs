using SimpleInjector;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System;

namespace App.Bootstrap.Extensions
{
    public static class BootstrapExtension
    {
        public static void UseBootstrap(this IServiceCollection services)
        {
            Container container = Ioc.RecoverContainer();

            services.AddSimpleInjector(container, options =>
            {
                options.AddAspNetCore()
                       .AddControllerActivation()
                       .AddViewComponentActivation();
            });
        }

        public static void UseBootstrap(this IApplicationBuilder app, Action<SimpleInjectorUseOptions> setupAction = null)
        {
            Container container = Ioc.RecoverContainer();

            app.UseSimpleInjector(container, setupAction);

            container.Verify();
        }
    }
}
