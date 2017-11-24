using System.IO;
using System.Reflection;
using HTF2017.Business;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using HTF2017.DataAccess;
using HTF2017.Mappers;

namespace HTF2017.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "HTF 2017 - Anonymous Androids", Version = "v1" });
                c.IncludeXmlComments(Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"));
            });

            services.AddEntityFrameworkSqlServer();

            services.AddTransient<HtfDbContext>();
            services.AddTransient<LocationLogic>();
            services.AddTransient<TeamLogic>();
            services.AddTransient<LocationMapper>();
            services.AddTransient<TeamMapper>();
            services.AddTransient<TeamRegistrationMapper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAll");
            app.UseMvc();
            app.UseSwagger(c =>
            {

            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HTF 2017 - Anonymous Androids - v1");
                c.DocExpansion("list");
            });
        }
    }
}