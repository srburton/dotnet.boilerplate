using SimpleInjector;
using App.Bootstrap;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using App.Bootstrap.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        readonly Container _container = Ioc.RecoverContainer();        

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.UseBootstrap();

            #region RabbitMq

            //_container.AddRabbitMQEventBus("amqp://", eventBusOptionAction: eventBusOption =>
            //{
            //    eventBusOption.ClientProvidedAssembly<Startup>();
            //    eventBusOption.EnableRetryOnFailure(true, 5000, TimeSpan.FromSeconds(30));
            //    eventBusOption.RetryOnFailure(TimeSpan.FromSeconds(1));
            //});

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            #region Ioc

            app.UseBootstrap(options =>
            {
                //options.UseMiddleware<>();
            });

            #endregion

            //app.RabbitMQAutoSubscribe(_container);
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
