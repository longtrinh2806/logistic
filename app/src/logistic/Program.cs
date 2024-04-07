using Data.Configurations;
using Data.DataAccess;
using Services.Core;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS configuration
builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<AppDbContext>();
builder.Services.Configure<AppDatabaseSetting>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<ICangNhapKhauService, CangNhapKhauService>();
builder.Services.AddScoped<IKhaiThacCangService, KhaiThacCangService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

try
{
    var dbContext = app.Services.GetRequiredService<AppDbContext>();
    dbContext.CreateCollectionsIfNotExisted();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    throw;
}

app.Run();
