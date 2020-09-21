using RestApiCore3.API.DbContexts;
using RestApiCore3.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using AutoMapper;
using System;

namespace RestApiCore3.API
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
           services.AddControllers(setupAction => {
               setupAction.ReturnHttpNotAcceptable = true;
               //setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
           }).AddXmlDataContractSerializerFormatters();//input and output

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IRestApiCore3Repository, RestApiCore3Repository>();

            services.AddDbContext<RestApiCore3Context>(options =>
            {
                options.UseSqlServer(
                    @"Server=(localdb)\ProjectsV13;Database=RestApiCore3DB;Trusted_Connection=True;");
            }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
