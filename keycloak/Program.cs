using keycloak;
using Keycloak.Auth.Api.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// this enables keycloak interaction in swagger without nedd of frontend
builder.Services.AddSwaggerGenWithAuth(builder.Configuration);

builder.Services.AddAuthorization();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.Audience = builder.Configuration["Keycloak:Audience"];
        o.MetadataAddress = builder.Configuration["Keycloak:MetadataAddress"];
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Keycloak:ValidIssuer"],
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuer = true
        };
    });

builder.Services.AddTransient<IClaimsTransformation, KeycloakRoleClaimsTransformation>();

builder.Services.AddAuthorization(options =>
{
    //options.AddPolicy("AdminPolicy", policy => policy.RequireRole("admin"));
    //options.AddPolicy("UserPolicy", policy => policy.RequireRole("user"));

    //// Policy requiring either "admin" or "user" or  roles
    //options.AddPolicy("MultiRolePolicy", policy =>
    //    policy.RequireRole("admin", "manager", "default-roles-test"));

    //// Policy requiring both "admin" and "supervisor" roles
    //options.AddPolicy("AdminAndSupervisorPolicy", policy =>
    //    policy.RequireAssertion(context =>
    //        context.User.IsInRole("admin") && context.User.IsInRole("supervisor")));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseMiddleware<TokenIntrospectionMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
