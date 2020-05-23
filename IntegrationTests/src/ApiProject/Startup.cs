using ApiProject.BusinessLogic;
using ApiProject.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;

namespace ApiProject
{
    public class Startup
    {
        //protected Container Container { get; private set; } = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			
            Bootstrap.Container = new Container();
            
            IntegrateSimpleInjector(services);

            RegisterSimpleInjectorDependencies(Bootstrap.Container);
        }

        private void RegisterSimpleInjectorDependencies(Container container)
        {
            container.Register<IValuesBusinessLogic, ValuesBusinessLogic>();
            container.Register<ValuesController>();
        }

        /// <summary>
        /// Allow SimpleInjector to participate in type registrations and injections in Asp.Net Core
        /// </summary>
        /// <param name="services"></param>
        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(Bootstrap.Container));
            services.UseSimpleInjectorAspNetRequestScoping(Bootstrap.Container);
            //services.EnableSimpleInjectorCrossWiring(Bootstrap.Container);
            services.AddSimpleInjector(Bootstrap.Container);
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

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
