using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using AutoMapper.Configuration;
using FuryTechs.BLM.EntityFrameworkCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FuryTechs.WebApi.Example.Db;
using FuryTechs.WebApi.Example.Models.Mapping;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace FuryTechs.WebApi.Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public IContainer ApplicationContainer { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<DatabaseContext>(o =>
            {
                o.UseMySql(Configuration.GetValue<string>("DbConnectionString"));
            });
            services.AddHttpContextAccessor();

            services.AddScoped<IIdentityResolver, IdentityResolver.IdentityResolver>();
            services.AddBlmEfCoreDefaultDbContext<DatabaseContext>();

            services.AddOData();
            services.AddSingleton(InitializeAutoMapper());

            var builder = new ContainerBuilder();

            builder.Populate(services);

            ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(ApplicationContainer);
        }

        /// <summary>
        /// Initialization method
        /// </summary>
        public IMapper InitializeAutoMapper()
        {
            var c = new MapperConfigurationExpression();
            c.AddMaps(Assembly.GetExecutingAssembly());
            Mapper.Initialize(c);
            var mapperConfig = new MapperConfiguration(c);
            mapperConfig.CompileMappings();
            return new Mapper(mapperConfig);
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseMvc(b =>
            {
                b.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                b.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller}/{action}/{id?}"
                );
                b.EnableDependencyInjection();
            });
        }
    }
}