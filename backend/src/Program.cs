/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */
 
using Core.Middlewares;
using Core.Models;
using Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

#region Builder creation

// ############################## Builder creation ##############################

if(!File.Exists(AppContext.BaseDirectory + "/appsettings.json"))
    throw new FileNotFoundException("The appsettings.json file is missing in the application root directory.");

var builder = WebApplication.CreateBuilder(args);
bool isDevelopment = builder.Environment.IsDevelopment();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Settings"));

#endregion



#region Services registration

// ############################## Services registration ##############################

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json")); // swagger
});

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(JWTHelper._secretKey)
            )
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            },
            OnForbidden = context =>
            {
                context.Response.StatusCode = 403;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Role.User, policy => policy.RequireAuthenticatedUser())
    .AddPolicy(Role.Admin, policy => policy.RequireAuthenticatedUser().RequireRole(Role.Admin));

// ############################## SWAGGER ##############################
if (isDevelopment)
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    builder.Services.AddSwaggerGenNewtonsoftSupport();
}

builder.Services.AddInjectableServices(Assembly.GetExecutingAssembly());

#endregion




var app = builder.Build();


#region App configuration

// ############################## App configuration ##############################

if (isDevelopment)
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.EnableTryItOutByDefault();
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

if (isDevelopment)
{
    app.UseCors(options =>
    {
        options
            .WithOrigins([
                "http://localhost:4200", // Angular ng serve
            ])
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
}

app.UseAuthentication();
app.UseAuthorization();

#endregion



#region App middleware

// ############################## App middleware ##############################

app.UseMiddleware<ErrorHandlingMiddleware>();

if(isDevelopment)
{
    app.UseMiddleware<RequestLoggerMiddleware>();
}

#endregion



#region App controllers

// ############################## App controllers ##############################

app.MapControllers();
app.MapFallbackToFile("/index.html"); // Serve Angular static files

#endregion

app.Run();
