using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetcoreApiTemplate.Data.Context;
using NetcoreApiTemplate.Data.Repositorys;
using NetcoreApiTemplate.Filters;
using NetcoreApiTemplate.Models;
using NetcoreApiTemplate.Services.Authen;
using System.Text;
using System.Text.Json.Serialization;

namespace NetcoreApiTemplate
{
    public class Program
    {
        private static IConfiguration? Configuration;
        readonly static string _pocicyName = "_myPolicyName";
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            Configuration = builder.Configuration;
            // Add services to the container.
            ConfigureServices(builder.Services);

            var app = builder.Build();

            InitializeMyDb(app);

            ConfigureMiddleware(app);

            app.Run();
        }
        public static void InitializeMyDb(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                SeedData.Initialize(services);
            }
        }
        private static void ConfigureServices(IServiceCollection services)
        {

            //services.AddDbContext<MyDbContext>(options => options.UseInMemoryDatabase(databaseName: "AuthorDb"));
            services.AddDbContext<MyDbContext>(options => options.UseInMemoryDatabase(databaseName: "my_database"));

            //services.AddDbContext<MyDbContext>(options => options.UseOracle(Configuration!.GetConnectionString("DbEis")));

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            #region services
            services.AddScoped<AuthorizeService>();
            #endregion
            #region repositorys
            services.AddScoped<CommonRepository>();
            services.AddScoped<EmployeeRepository>();

            #endregion

            #region swagger
            services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();
            services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new string[]{}
                    }
                });
            });
            #endregion

            services.AddHealthChecks();

            services.AddCors(options =>
            {
                options.AddPolicy(name: _pocicyName,
                    policy =>
                    {
                        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            });

            services.AddControllers(o => o.Filters.Add(new HttpResponseExceptionFilter()))
             .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.PropertyNamingPolicy = null;
                 options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                 options.JsonSerializerOptions.WriteIndented = true;
             }
            );

            #region JWT Token
            var key = Configuration!.GetSection("JwtConfig:JwtSecretKey").Get<string>() ?? string.Empty;
            var jwtWebSiteDomain = Configuration.GetSection("JwtConfig:JwtWebSiteDomain").Get<string>();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(token =>
             {
                 token.RequireHttpsMetadata = false;
                 token.SaveToken = true;
                 token.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                     ValidateIssuer = true,
                     ValidIssuer = jwtWebSiteDomain,
                     ValidateAudience = true,
                     ValidAudience = jwtWebSiteDomain,
                     RequireExpirationTime = true,
                     ValidateLifetime = true,
                 };
             });

            #endregion

        }
        private static void ConfigureMiddleware(WebApplication app)
        {
            #region pathbase support k8s
            var pathBase = Configuration!["PATH_BASE"]; // -- env runtime
            if (!string.IsNullOrWhiteSpace(pathBase))
            {
                pathBase = $"/{pathBase.TrimStart('/')}";
                app.UsePathBase(pathBase);
            }
            #endregion

            if (app.Environment.IsDevelopment()) //-- hide swagger in PROD
                app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseCors(_pocicyName);
            app.MapHealthChecks("/healthcheck").ShortCircuit();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
