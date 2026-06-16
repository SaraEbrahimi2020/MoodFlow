using Microsoft.EntityFrameworkCore;
using MoodFlow.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); // برای شناسایی مسیرها
builder.Services.AddSwaggerGen();           // برای ساختن مستندات سواگر
// این خط به MediatR می‌گوید برو توی پروژه Application و تمام کامندها و کوئری‌ها را پیدا کن
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MoodFlow.Application.Features.MoodLogs.Commands.RegisterMoodLogCommand).Assembly));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

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
