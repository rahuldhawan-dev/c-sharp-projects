using DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace CachingProofOfConcept
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddTransient<ICountryRepository, SqlCachingCountryRepository>();
			services.AddTransient<IDataContext>( r => new SqlServerDataContext(Configuration["ConnectionString"], 200, NullLogger<SqlServerDataContext>.Instance));

			// Registered as singleton so can leverage StackExchange.Redis.ConnectionMultiplexer
			services.AddSingleton<IAppCache, RedisAppCache>();

			services.Configure<RedisCacheOptions>(Configuration.GetSection("RedisCache"));
			services.Configure<CacheTtlOptions>(Configuration.GetSection("CacheTTL"));

			// Prime Cache: this would avoid caching request sent from multiple places in the service
			// at the same time as it would be already cached
			services.AddSingleton<IHostedService, PrimeCachingService>(); 
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseStaticFiles();
			app.UseMvc();
		}
	}
}
