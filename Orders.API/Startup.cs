using System;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Model;
using Orders.API.Controllers.MapModels;
using SharedData;

namespace Orders.API
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
            services.ConfigureCors(Configuration).ConfigureSwagger(Configuration).ConfigureContext(Configuration)
                .ConfigureAuthService(Configuration).ConfigureControllers(Configuration).ConfigureMapper(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();


            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orders API V1"); });
        }
    }

    public static class CustomExtensionMethods
    {
        public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000",
                            "https://localhost:3001");
                        builder.WithOrigins("http://localhost:3003",
                            "https://localhost:3003").AllowAnyHeader();
                    });
            });
            return services;
        }


        public static IServiceCollection ConfigureAuthService(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();
            var builder = services.AddIdentityCore<ApplicationUser>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddEntityFrameworkStores<DataContext>();
            identityBuilder.AddSignInManager<SignInManager<ApplicationUser>>();
            identityBuilder.AddUserManager<UserManager<ApplicationUser>>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            }).AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(configuration["UserValidation:Secret"]);

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("JwtAuth", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy("CookieAuth", policy =>
                {
                    policy.AuthenticationSchemes.Add(CookieAuthenticationDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
            });
            return services;
        }

        public static IServiceCollection ConfigureContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );
            return services;
        }

        public static IServiceCollection ConfigureControllers(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateFilter));
                options.Filters.Add(new AuthorizeFilter("JwtAuth"));
                options.Filters.Add(new AuthorizeFilter("CookieAuth"));
            });
            return services;
        }

        public static IServiceCollection ConfigureMapper(this IServiceCollection services, IConfiguration configuration)
        {
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }


        public static IServiceCollection ConfigureSwagger(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Orders.API", Version = "v1"});
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
            return services;
        }

        public class ValidateModelStateFilter : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                if (!context.ModelState.IsValid) context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}