using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Enuns;
using MinimalApi.Domain.Interfaces;
using MinimalApi.Domain.ModelViews;
using MinimalApi.Domain.Services;
using MinimalApi.Infrastructure.Db;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            key = Configuration?.GetSection("Jwt")?.ToString() ?? "";
        }

        private string key = "";
        public IConfiguration Configuration { get; set; } = default!;

        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            services.AddAuthorization();

            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IVehicleService, VehicleService>();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token here"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new String[] {}
                    }
                });
            });

            services.AddDbContext<MinimalApiContext>(options =>
            {
                options.UseMySql(
                    Configuration.GetConnectionString("MySql"),
                    ServerVersion.AutoDetect(Configuration.GetConnectionString("MySql"))
                );
            });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                #region Home
                endpoints.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
                #endregion

                #region Admins
                string JwtTokenGenerate(Admin admin)
                {
                    if (string.IsNullOrEmpty(key)) return string.Empty;

                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new List<Claim>()
                {
                    new Claim("Email", admin.Email),
                    new Claim("Role", admin.Role),
                    new Claim(ClaimTypes.Role, admin.Role)
                };

                    var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddDays(1),
                        signingCredentials: credentials
                        );

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }

                endpoints.MapPost("/admin/login", ([FromBody] LoginDTO loginDTO, IAdminService adminService) =>
                {
                    var admin = adminService.Login(loginDTO);
                    if (admin != null)
                    {
                        string token = JwtTokenGenerate(admin);
                        return Results.Ok(new AdminLoggedIn
                        {
                            Email = admin.Email,
                            Role = admin.Role,
                            Token = token
                        });
                    }
                    else
                        return Results.Unauthorized();
                }).AllowAnonymous().WithTags("Admins");

                endpoints.MapPost("/admin", ([FromBody] AdminDTO adminDTO, IAdminService adminService) =>
                {
                    var validation = new ErrorValidation { Messages = new List<string>() };

                    if (string.IsNullOrEmpty(adminDTO.Email))
                        validation.Messages.Add("Email cannot be empty");
                    if (string.IsNullOrEmpty(adminDTO.Password))
                        validation.Messages.Add("Password cannot be empty");
                    if (adminDTO.Role == null)
                        validation.Messages.Add("Role cannot be empty");
                    if (validation.Messages.Count > 0)
                        return Results.BadRequest(validation);

                    var admin = new Admin
                    {
                        Email = adminDTO.Email,
                        Password = adminDTO.Password,
                        Role = adminDTO.Role.ToString() ?? Role.Editor.ToString()
                    };

                    adminService.AddAdmin(admin);

                    var adminModelView = new AdminModelView
                    {
                        id = admin.Id,
                        Email = admin.Email,
                        Role = admin.Role
                    };

                    return Results.Created($"/admin/{admin.Id}", adminModelView);
                })
                    .RequireAuthorization()
                    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
                    .WithTags("Admins");

                endpoints.MapGet("/admins", ([FromQuery] int? page, IAdminService adminService) =>
                {
                    var admins = new List<AdminModelView>();
                    var adms = adminService.All(page);
                    foreach (var adm in adms)
                    {
                        admins.Add(new AdminModelView
                        {
                            id = adm.Id,
                            Email = adm.Email,
                            Role = adm.Role
                        });
                    }

                    return Results.Ok(admins);
                })
                    .RequireAuthorization()
                    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
                    .WithTags("Admins");

                endpoints.MapGet("/admin/{id}", ([FromRoute] int id, IAdminService adminService) =>
                {
                    var admin = adminService.GetAdmin(id);
                    if (admin == null) return Results.NotFound();

                    var adminModelView = new AdminModelView
                    {
                        id = admin.Id,
                        Email = admin.Email,
                        Role = admin.Role
                    };

                    return Results.Ok(adminModelView);
                })
                    .RequireAuthorization()
                    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
                    .WithTags("Admins");
                #endregion

                #region Vehicles
                ErrorValidation errorValidationDTO(VehicleDTO vehicleDTO)
                {
                    var errorValidation = new ErrorValidation
                    {
                        Messages = new List<string>()
                    };

                    if (string.IsNullOrEmpty(vehicleDTO.Name))
                        errorValidation.Messages.Add("Name cannot be empty");
                    if (string.IsNullOrEmpty(vehicleDTO.Brand))
                        errorValidation.Messages.Add("Brand cannot be empty");
                    if (vehicleDTO.Year < 1950)
                        errorValidation.Messages.Add("Very old vehicle, only accepted from years old than 1950");

                    return errorValidation;
                }

                endpoints.MapPost("/vehicle", ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
                {
                    var errorValidation = errorValidationDTO(vehicleDTO);
                    if (errorValidation.Messages.Count > 0)
                        return Results.BadRequest(errorValidation);

                    var vehicle = new Vehicle
                    {
                        Name = vehicleDTO.Name,
                        Brand = vehicleDTO.Brand,
                        Year = vehicleDTO.Year
                    };
                    vehicleService.AddVehicle(vehicle);
                    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);
                })
                    .RequireAuthorization()
                    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
                    .WithTags("Vehicles");

                endpoints.MapGet("/vehicle", ([FromQuery] int? page, IVehicleService vehicleService) =>
                {
                    var vehicles = vehicleService.All(page);

                    return Results.Ok(vehicles);
                }).RequireAuthorization().WithTags("Vehicles");

               endpoints.MapGet("/vehicle/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
                {
                    var vehicle = vehicleService.GetVehicle(id);

                    if (vehicle == null) return Results.NotFound();
                    return Results.Ok(vehicle);
                })
                    .RequireAuthorization()
                    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
                    .WithTags("Vehicles");

                endpoints.MapPut("/vehicle/{id}", ([FromRoute] int id, VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
                {
                    var vehicle = vehicleService.GetVehicle(id);
                    if (vehicle == null) return Results.NotFound();

                    var errorValidation = errorValidationDTO(vehicleDTO);
                    if (errorValidation.Messages.Count > 0)
                        return Results.BadRequest(errorValidation);

                    vehicle.Name = vehicleDTO.Name;
                    vehicle.Brand = vehicleDTO.Brand;
                    vehicle.Year = vehicleDTO.Year;

                    vehicleService.UpdateVehicle(vehicle);

                    return Results.Ok(vehicle);
                })
                    .RequireAuthorization()
                    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
                    .WithTags("Vehicles");

                endpoints.MapDelete("/vehicle/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
                {
                    var vehicle = vehicleService.GetVehicle(id);
                    if (vehicle == null) return Results.NotFound();

                    vehicleService.DeleteVehicle(vehicle);

                    return Results.NoContent();
                })
                    .RequireAuthorization()
                    .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
                    .WithTags("Vehicles");
                #endregion
            });
        }
    }
}
