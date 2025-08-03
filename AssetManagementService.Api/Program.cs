using Microsoft.EntityFrameworkCore;
using AssetManagementService.Infrastructure.Data;
using AssetManagementService.Application.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationValidators();

builder.Services.AddDbContext<AssetManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://authservice.yourdomain.com";
        options.Audience = "asset_management_api";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = "asset_management_api",

            ValidateIssuer = true,
            ValidIssuer = "https://authservice.yourdomain.com",

            ValidateLifetime = true
        };

        options.RequireHttpsMetadata = true;
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.UseCors(builder =>
    builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();