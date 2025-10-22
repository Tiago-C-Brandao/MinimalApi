
using MinimalApi.Domain.DTOs;

namespace MinimalApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/", () => "Hello World!");

            app.MapPost("/login", (LoginDTO loginDTO) =>
            {
                if (loginDTO.Email == "admin@teste.com" && loginDTO.Password == "123456")
                    return Results.Ok("Login com sucesso");
                else
                    return Results.Unauthorized();
            });

            app.Run();
        }
    }
}
