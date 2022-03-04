using DotNetCore_Filters.Data;
using DotNetCore_Filters.Filters.ActionFilters;
using DotNetCore_Filters.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore_Filters
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
            //filter can be added to different scope levels: Global, Action, Controller

            #region Add Filter at global level
            //to register action filter at global level so each request will be tracked
            //services.AddControllers(a =>
            //{
            //    //by an instance
            //    a.Filters.Add(new ActionFilterExample());
            //    //by the type
            //    a.Filters.Add(typeof(ActionFilterExample));
            //});

            #endregion Add Filter at global level

            #region Add Filter as a service on action or controller level
            //filter as a service type on the Action or Controller level,
            //services.AddScoped<ActionFilterExample>();
            //services.AddScoped<SampleAsyncActionFilter>();
            //services.AddScoped<ActionFilterWithCustom>();
            services.AddScoped<ActionFilterWithLogger>();

            #endregion  Add Filter as a service on action or controller level

            services.AddControllers();

            services.AddDbContext<DatabaseContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:MyConnection"]));
            services.AddTransient<ITokenManager, Auth.TokenManager>();
            services.AddTransient<IUserLoginManager, Data.UserLoginManager>();
            services.AddTransient<ILogFactory, LogFactory>();

            //Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            //Addding Jwt Bearer
            .AddJwtBearer(options =>
            {
                //Suppose a token is not passed through header and passed through the url query string
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Query.ContainsKey("access_token"))
                        {
                            context.Token = context.Request.Query["access_token"];
                        }
                        return Task.CompletedTask;
                    }
                };


                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DotNetCore_Filters", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNetCore_Filters v1"));
            }

            app.UseRouting();

            app.UseAuthentication();

            #region Create own Authorize 
            // create own authorization like below
            //app.Use(async (context, next) =>
            //{
            //    if (!context.User.Identity?.IsAuthenticated ?? false)
            //    {
            //        //Microsoft.AspNetCore.Http.
            //        context.Response.StatusCode = 401;
            //        await context.Response.WriteAsync("Not Authorized");
            //    }
            //    else
            //        await next();
            //});
            #endregion Create own Authorize 

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
