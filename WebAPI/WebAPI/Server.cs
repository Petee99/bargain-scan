// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Server.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region Imports

using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using WebAPI.Interfaces;
using WebAPI.Models.Authentication;
using WebAPI.Models.DataModels;
using WebAPI.Models.WebScraping;
using WebAPI.Properties;
using WebAPI.Services;

#endregion


#region Fields

string[] requestMethods =
{
    Constants.GetMethod,
    Constants.PostMethod,
    Constants.PutMethod,
    Constants.DeleteMethod,
    Constants.OptionsMethod
};

#endregion


#region Builder

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#endregion


#region Services

// Add services to the container.

builder.Services.AddCors(options => options.AddPolicy(Constants.CorsPolicyKey, b => b
    .WithOrigins(Constants.ClientDomain, Constants.ClientDomain)
    .SetIsOriginAllowedToAllowWildcardSubdomains()
    .AllowAnyHeader()
    .AllowCredentials()
    .WithMethods(requestMethods)
    .SetPreflightMaxAge(TimeSpan.FromSeconds(Constants.PreFlightMaxTimeInSeconds)
    )));

builder.Services.AddSingleton<IMongoDbService, MongoDbService>();
builder.Services.AddSingleton<IDataBaseService<UserModel>, DataBaseService<UserModel>>();
builder.Services.AddSingleton<IDataBaseService<AdminModel>, DataBaseService<AdminModel>>();
builder.Services.AddSingleton<IDataBaseService<ShopItem>, DataBaseService <ShopItem>>();
builder.Services.AddSingleton<IDataBaseService<ShopItemCategory>, DataBaseService<ShopItemCategory>>();
builder.Services.AddSingleton<IDataBaseService<ScrapeRequest>, DataBaseService<ScrapeRequest>>();
builder.Services.AddSingleton<IScraperService, ScraperService>();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.RequireHttpsMetadata = false;
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable(Constants.JwtKeyVariable)!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    option.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies[Constants.AccessToken];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

WebApplication app = builder.Build();

app.UseCors(b => b
    .WithOrigins(Constants.ClientDomain, Constants.ClientDomain)
    .SetIsOriginAllowedToAllowWildcardSubdomains()
    .AllowAnyHeader()
    .AllowCredentials()
    .WithMethods(requestMethods)
    .SetPreflightMaxAge(TimeSpan.FromSeconds(Constants.PreFlightMaxTimeInSeconds)
    ));

#endregion


#region Pipeline

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

#endregion