using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ApiProject.Tests.Setup
{
    public class TestFixture<T> : IDisposable
    {
        private readonly TestServer _server;

        protected IConfiguration _config;

        public HttpClient Client { get; }

        public IServiceProvider Container { get; private set; }

        public TestFixture()
        {
            var environment = "InteractionTest";

            // WebHostBuilder always calls Startup.cs's configuration methods *last*,
            // So to replace the dependencies in there, we have to get fancy.
            // See: https://github.com/aspnet/Hosting/issues/905

            var builder = new WebHostBuilder()
                .UseConfiguration(UseConfiguration(environment))
                .ConfigureServices((context, collection) =>
                {
                    collection.AddTransient<IServiceProviderFactory<IServiceCollection>>(r =>
                    {
                        return new TestServiceProviderFactory(x =>
                        {
                            ConfigureServices(context, x);
                        });
                    });

                    // IMPORTANT:
                    // do not reduce this to a method group, 
                    // or it will not fire, likely due to poor cached delegate strategy
                    // ReSharper disable once ConvertClosureToMethodGroup
                    // ConfigureServices(context, collection);
                })
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    // ReSharper disable once ConvertClosureToMethodGroup
                    ConfigureAppConfiguration(context, configurationBuilder);
                })
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    // ReSharper disable once ConvertClosureToMethodGroup
                    ConfigureLogging(context, loggingBuilder);
                })
                .UseEnvironment(environment)
                .UseStartup(typeof(T));

            _server = new TestServer(builder);
            Client = _server.CreateClient();
            Container = _server.Host.Services;
        }

        public class TestServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
        {
            private readonly Action<IServiceCollection> _closure;
            private readonly ServiceProviderOptions _options;

            public TestServiceProviderFactory(Action<IServiceCollection> closure) : this(new ServiceProviderOptions())
            {
                _closure = closure;
            }

            public TestServiceProviderFactory(ServiceProviderOptions options)
            {
                _options = options ?? throw new ArgumentNullException(nameof(options));
            }

            public IServiceCollection CreateBuilder(IServiceCollection services)
            {
                return services;
            }

            public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
            {
                _closure(containerBuilder);

                return containerBuilder.BuildServiceProvider(_options);
            }
        }

        private IConfiguration UseConfiguration(string environment)
        {
            _config = BuildConfiguration(environment);

            return _config;
        }

        protected virtual IConfiguration BuildConfiguration(string environment)
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile($"appsettings.{environment}.json", true, true);
            config.AddEnvironmentVariables();
            return config.Build();
        }

        protected virtual void ConfigureServices(WebHostBuilderContext context, IServiceCollection services) { }

        protected virtual void ConfigureLogging(WebHostBuilderContext hostingContext, ILoggingBuilder logging)
        {
            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            logging.AddConsole();
        }

        protected virtual void ConfigureAppConfiguration(WebHostBuilderContext hostingContext, IConfigurationBuilder config) { }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }
    }
}