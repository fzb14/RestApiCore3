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
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;

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
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                //setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            })
                 .AddJsonOptions(o => o.JsonSerializerOptions.WriteIndented = true)
                .AddXmlDataContractSerializerFormatters()//input and output
                .AddNewtonsoftJson(setupAction =>
                {
                    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                })
                .ConfigureApiBehaviorOptions(setupAction => {
                    setupAction.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetailFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                        var problemDetail = problemDetailFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);
                        problemDetail.Detail = "see the error field for details";
                        problemDetail.Instance = context.HttpContext.Request.Path;
                        var actionExecutingContext = context as ActionExecutingContext;
                        if ((context.ModelState.ErrorCount>0)&&(actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
                        {
                            problemDetail.Type = "https://courselibrary.com/modelvalidationproblem";
                            problemDetail.Status = StatusCodes.Status422UnprocessableEntity;
                            problemDetail.Title = "One or more validation errors occured.";
                            return new UnprocessableEntityObjectResult(problemDetail)
                            {
                                ContentTypes = {"application/problem+json" }
                            };
                        }
                        //if one of the arguments wasn't correctly found / couldn't be parsed
                        //we're dealing with null/unparseable input
                        problemDetail.Status = StatusCodes.Status400BadRequest;
                        problemDetail.Title = "One or more errors on input occured.";
                        return new BadRequestObjectResult(problemDetail)
                        {
                            ContentTypes = { "application/problem+json" }
                        };
                    };
                    
                });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IRestApiCore3Repository, RestApiCore3Repository>();

            services.AddDbContext<RestApiCore3Context>(options =>
            {
                options.UseSqlServer(
                    @"Server=(localdb)\ProjectsV13;Database=RestApiCore3DB;Trusted_Connection=True;"
                );
            }); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder => {
                    appBuilder.Run(async context => {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("an exception happened. try again later.");
                    });
                });
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
