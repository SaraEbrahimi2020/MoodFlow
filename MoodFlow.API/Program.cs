using MediatR;
using Microsoft.EntityFrameworkCore;
using MoodFlow.API.Middlewares;
using MoodFlow.Application;
using MoodFlow.Infrastructure;
using MoodFlow.Infrastructure.Persistence;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); // برای شناسایی مسیرها-
builder.Services.AddSwaggerGen();
// ثبت تمام سرویس‌های لایه اپلیکیشن (مدیاآر، ولیدیشن و بیهیویرها) به صورت یکجا
builder.Services.AddApplicationServices();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
var app = builder.Build();
// حتماً باید بالای مسیردهی کنترلرها باشد
app.UseExceptionHandler();
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
