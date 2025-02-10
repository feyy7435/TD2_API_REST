using Microsoft.EntityFrameworkCore;
using TD2_API_REST.Models.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRazorPages();


builder.Services.AddDbContext<SeriesDbContext>(options=>options.UseNpgsql(builder.Configuration.GetConnectionString("SeriesDB")));
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(policy =>
    policy.WithOrigins("https://localhost:7012;http://localhost:5230")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
);
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
