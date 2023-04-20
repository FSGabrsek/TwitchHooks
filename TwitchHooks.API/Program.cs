using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TwitchHooks.Application.Network;
using TwitchHooks.Domain.Repositories;
using TwitchHooks.Infrastructure;
using TwitchHooks.Infrastructure.Repositories;
using TwitchHooksAPI.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IWebhookRepository, WebhookRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ITwitchApiClient, TwitchApiClient>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
    {
        config.Password.RequiredLength = 8;
        config.Password.RequireDigit = true;
        config.Password.RequireUppercase = false;
        config.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(o =>
    o.AddPolicy("RequireAdmin", p => p.RequireClaim("Claim.Admin")));
builder.Services.AddAuthorization(o =>
    o.AddPolicy("RequireMember", p => p.RequireClaim("Claim.Member")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "TwitchHooks",

        ValidateAudience = true,
        ValidAudience = "TwitchHooks",

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            "developmentkey"u8.ToArray())
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(
    (provider, optionsBuilder) =>
    {
        var options = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
        optionsBuilder.UseNpgsql(options.ConnectionString, contextOptionsBuilder =>
        {
            contextOptionsBuilder.EnableRetryOnFailure(options.MaxRetryCount);
            contextOptionsBuilder.CommandTimeout(options.CommandTimeout);
        });
        optionsBuilder.EnableDetailedErrors(options.EnableDetailedErrors);
        optionsBuilder.EnableSensitiveDataLogging(options.EnableSensitiveDataLogging);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
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
