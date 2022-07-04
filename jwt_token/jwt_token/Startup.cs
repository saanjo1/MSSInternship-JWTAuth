using jwt_token.AzureRepo;
using JWTAuth_Validation.Middleware;
//using jwt_token.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace jwt_token
{

    public class Startup
    {
        public string _azureConnectionString = Environment.GetEnvironmentVariable("AZURE_CONNECTION_STRING");
        public string _azureTableName= Environment.GetEnvironmentVariable("AZURE_TABLE_NAME");

        public string _registeredAzureConnectionString = Environment.GetEnvironmentVariable("REGISTERED_AZURE_CONNECTION_STRING");
        public string _registeredAzureTableName = Environment.GetEnvironmentVariable("REGISTERED_AZURE_TABLE_NAME");
        public string _roleAzureConnectionString = Environment.GetEnvironmentVariable("ROLE_AZURE_CONNECTION_STRING");
        public string _roleAzureTableName = Environment.GetEnvironmentVariable("ROLE_AZURE_TABLE_NAME");
        public string _roleuserAzureConnectionString = Environment.GetEnvironmentVariable("USERROLE_AZURE_CONNECTION_STRING");
        public string _roleuserAzureTableName = Environment.GetEnvironmentVariable("USERROLE_AZURE_TABLE_NAME");
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc()
             .AddSessionStateTempDataProvider();
            services.AddSession();
            services.AddControllers();
            services.AddSingleton<IAzureRepo<Models.RegisterUser>>(repo => new UserRepository(_registeredAzureConnectionString, _registeredAzureTableName));
            services.AddSingleton<IAzureRepo<Models.Role>>(repo => new RolesRepository(_roleAzureConnectionString, _roleAzureTableName));
            services.AddSingleton<IAzureRepo<Models.UserRoles>>(repo => new UserRoleRepository(_roleuserAzureConnectionString, _roleuserAzureTableName));

            //services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserRoleService, UserRoleService>();


            var secret = System.Environment.GetEnvironmentVariable("SECRET");

            var key = Encoding.ASCII.GetBytes(secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddTransient<IUserService, UserService>();

            #region Swagger Configuration
            services.AddSwaggerGen(swagger =>
            {
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "JWT Token Authentication API",
                    Description = "ASP.NET Core 5.0 Web API"
                });
                // To Enable authorization using Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] {}
                    }
                });
            });
            #endregion

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseHttpsRedirection();

            app.UseMiddleware<JWTMiddleware>();



            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "jwt_token v1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        
           
        }



  
    }
}
