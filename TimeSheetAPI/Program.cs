using Microsoft.EntityFrameworkCore;
using TimeSheetAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injektiolla v�litetty tietokantatieto kontrollereille
builder.Services.AddDbContext<tuntidbContext>(options => options.UseSqlServer(
   //builder.Configuration.GetConnectionString("paikallinen")
    builder.Configuration.GetConnectionString("pilvi")
    ));

//Cors m��ritys + alas app.UseCors("all") kohtaan
builder.Services.AddCors(options =>
{
    options.AddPolicy("all",
    builder => builder.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("all"); //Cors k�ytt��notto

app.UseAuthorization();

app.MapControllers();

app.Run();
