using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TisaBackend.BL.Services;
using TisaBackend.DAL;
using TisaBackend.DAL.Repositories;
using TisaBackend.Domain.Auth;
using TisaBackend.Domain.Interfaces;
using TisaBackend.Domain.Interfaces.BL;
using TisaBackend.Domain.Interfaces.DAL;

namespace TisaBackend.WebApi
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
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling =
                            Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    }
            );

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            //TODO: check registration types
            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton<IAirportRepository, AirportRepository>();
            services.AddSingleton<IAirlineRepository, AirlineRepository>();
            services.AddSingleton<IFlightRepository, FlightRepository>();
            services.AddSingleton<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IUserService, UserService>();

            services.AddSingleton<IAirportService, AirportService>();
            services.AddSingleton<IAirplaneTypeService, AirplaneTypeService>();

            services.AddSingleton<IAirlineService, AirlineService>();
            services.AddSingleton<IFlightService, FlightService>();

            ConfigureDal(services);
            
            ConfigureAuth(services);

            ConfigureSwagger(services);
        }

        public void ConfigureDal(IServiceCollection services)
        {
            //Entity Framework
            services.AddDbContext<TisaContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("Postgres")));
        }

        public void ConfigureAuth(IServiceCollection services)
        {
            //Identity
            services.AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 7;
                })
                .AddEntityFrameworkStores<TisaContext>()
                .AddDefaultTokenProviders();

            // Adding Authentication  
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                // Adding Jwt Bearer  
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = Configuration["JWT:ValidAudience"],
                        ValidIssuer = Configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                    };
                });
        }

        public void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TisaBackend.WebApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
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
                        new string[] {}

                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // This is middleware with chronological order!!

            // use url routing based on controllers functions (to find them)
            app.UseRouting();

            app.UseCors("MyPolicy");

            // who are you?
            app.UseAuthentication();

            // are you allowed?
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TisaBackend.WebApi v1"));

            // if you pass auth steps, then execute the function that you find in the route
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}