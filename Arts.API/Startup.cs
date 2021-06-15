using System;
using System.IO;
using System.Reflection;
using System.Text;
using Arts.API.Controllers.MapModels;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Model;
using SharedData;

namespace ArtWorkGallery.API
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
            services.AddHttpContextAccessor();
            services.ConfigureCors(Configuration).ConfigureControllers(Configuration).ConfigureSwagger(Configuration)
                .ConfigureContext(Configuration).AddCustomAuthentication(Configuration).ConfigureMapper(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                if (env.IsDevelopment())
                    endpoints.MapControllers().WithMetadata(new AllowAnonymousAttribute());
                else
                    endpoints.MapControllers();
            });
            app.UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gallery API V1"); });
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
                        builder.WithOrigins("http://localhost:3002",
                            "https://localhost:3002");
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

        public static IServiceCollection ConfigureMapper(this IServiceCollection services, IConfiguration configuration)
        {
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }


        public static IServiceCollection ConfigureControllers(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateFilter));
                options.Filters.Add(new AuthorizeFilter());
            });
            return services;
        }


        public static IServiceCollection ConfigureSwagger(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Arts.API", Version = "v1"});
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
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
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

        public class HttpResponseException : Exception
        {
            public int Status { get; set; } = 500;

            public object Value { get; set; }
        }
        
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();
            var builder = services.AddIdentityCore<ApplicationUser>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddEntityFrameworkStores<DataContext>();
            identityBuilder.AddSignInManager<SignInManager<ApplicationUser>>();
            identityBuilder.AddUserManager<UserManager<ApplicationUser>>();
            
            var identityUrl = configuration.GetValue<string>("identityUrl");
            var apiSecret = configuration.GetValue<string>("apiSecret");
            var name = configuration.GetValue<string>("resName");
            services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                        
                    }
                   )
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = identityUrl;
                    options.ApiName = name;
                    options.ApiSecret = apiSecret;
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.CacheDuration = TimeSpan.FromMinutes(30);

            
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyFromOrders",
                    policy =>
                    {
                        policy.RequireClaim("scope","api.orders");
                    });
                options.AddPolicy("OnlyFromStats",
                    policy =>
                    {
                        policy.RequireClaim("scope","api.stats");
                    });
            });
            services.AddControllers();
            return services;
        }
    }
}