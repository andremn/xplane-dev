using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VueCliMiddleware;
using XPDev.FlightManagement;
using XPDev.FlightSoundsManagement.Configuration;
using XPDev.Modularization;

using FlightManagementModuleEntry = XPDev.FlightManagement.ModuleEntry;
using FlightSoundsManagementModuleEntry = XPDev.FlightSoundsManagement.ModuleEntry;

namespace XPDev.FlightSoundsSimulator
{
    public class Startup
    {
        private static readonly string _fmodDllPath = Path.Combine(AppContext.BaseDirectory, "fmod64.dll");

        private IList<ModuleEntryBase> _modules;

        public Startup(IConfiguration configuration)
        {
            _modules = new List<ModuleEntryBase>();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var flightParametersService = new FlightParametersService();
            var moduleFactory = new ModuleFactory(services);

            services.AddSingleton<IModulesManager, ModulesManager>();
            services.AddSingleton<IFlightSnapshotProvider>(flightParametersService);
            services.AddSingleton<IFlightParametersService>(flightParametersService);
            services.AddSingleton<IFlightParameterRequestProcessor, FlightParameterRequestProcessor>();
            services.AddSingleton<IFlightSoundConfigurationProvider>(new FlightSoundConfigurationProvider(_fmodDllPath));

            _modules.Add(moduleFactory.CreateInstance<FlightManagementModuleEntry>());
            _modules.Add(moduleFactory.CreateInstance<FlightSoundsManagementModuleEntry>());

            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IModulesManager modulesManager)
        {
            modulesManager.AddModules(_modules);
            AsyncContext.Run(modulesManager.StartModulesAsync);

            Task.Run(async () =>
            {
                while (true)
                {
                    modulesManager.Run();
                    await Task.Delay(1000);
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseSpaStaticFiles();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = env.IsDevelopment() ? "ClientApp" : "dist";

                if (env.IsDevelopment())
                {
                    spa.UseVueCli(npmScript: "serve");
                }
            });
        }
    }
}
