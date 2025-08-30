
using CRUD.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        #region Configuracion de la base de datos -- Conexion a SQL Server
        builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=LocalConnection"));
        #endregion

        #region Configuracion de CORS -- Permitir conexiones desde el frontend
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                // Allow specific origins for CORS or development purposes from localhost of the frontend
                policy.WithOrigins("https://localhost:7010")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });

        });
        #endregion


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AllowSpecificOrigins");
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
