using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ACS.API.Authentication;
using ACS.API.Authentication.Services;
using ACS.Core.Interfaces;
using ACS.Infrastructure.Data;
using ACS.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Autofac;
using System.Reflection;
using ACS.Core.Interfaces.UseCases;
using Autofac.Extensions.DependencyInjection;
using ACS.Infrastructure.Data.Repositories;
using ACS.Core.Interfaces.Repositories;
using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace ACS.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            IdentityModelEventSource.ShowPII = true;
            services.AddSingleton<ITokenService, TokenService>();
            services.AddDbContext<ACSDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            var jwtOptions = Configuration.GetSection(nameof(JwtOptions));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions[nameof(JwtOptions.SigningKey)]));

            services.Configure<JwtOptions>(options =>
            {
                options.Issuer = jwtOptions[nameof(JwtOptions.Issuer)];
                options.Audience = jwtOptions[nameof(JwtOptions.Audience)];
                options.SigningKey = jwtOptions[nameof(JwtOptions.SigningKey)];
                options.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions[nameof(JwtOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtOptions[nameof(JwtOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.ClaimsIssuer = jwtOptions[nameof(JwtOptions.Issuer)];
                options.TokenValidationParameters = tokenValidationParameters;
                options.SaveToken = true;

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = ctx =>
                    {
                        if (ctx.Exception is SecurityTokenExpiredException)
                        {
                            ctx.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Api Usage", policy => policy.RequireClaim(ApiClaims.ClaimIdentifiers.Rol, ApiClaims.ClaimValues.ApiAccess));
            });

            services.AddIdentity<AppUser, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<ACSDbContext>().AddDefaultTokenProviders();

            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IUseCaseRequestHandler<,>))).Where(x => x.Name.EndsWith("UseCase") && x.IsClass).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(x => x.Name.EndsWith("Presenter")).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.Populate(services);
            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ACS API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseExceptionHandler(
                builder =>
                {
                    builder.Run(
                        async ctx =>
                        {
                            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            ctx.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                            var err = ctx.Features.Get<IExceptionHandlerFeature>();

                            if (err != null)
                            {
                                ctx.Response.Headers.Add("Application-Error", Regex.Replace(err.Error.Message, @"\p{C}+", string.Empty));
                                ctx.Response.Headers.Add("access-control-expose-headers", "Application-Error");

                                await ctx.Response.WriteAsync(err.Error.Message).ConfigureAwait(false);
                            }
                        });
                });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
