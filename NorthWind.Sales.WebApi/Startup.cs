using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
// Ajusta si tu extensión AddSwaggerGenBearer está en otro namespace
using System.Text;

namespace NorthWind.Sales.WebApi;

internal static class Startup
{
    public static WebApplication CreateWebApplication(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGenBearer();
        builder.Services.AddNorthWindSalesServices(
            dbOptions =>
                builder.Configuration.GetSection(DBOptions.SectionKey)
                    .Bind(dbOptions),
            smtpOptions =>
                builder.Configuration.GetSection(SmtpOptions.SectionKey)
                    .Bind(smtpOptions),
            membershipDBOptions =>
                builder.Configuration.GetSection(MembershipDBOptions.SectionKey)
                    .Bind(membershipDBOptions),
            jwtOptions =>
                builder.Configuration.GetSection(JwtOptions.SectionKey)
                    .Bind(jwtOptions));

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(config =>
            {
                config.AllowAnyMethod();
                config.AllowAnyHeader();
                config.AllowAnyOrigin();
            });
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Establecer la configuración del Token.
                builder.Configuration.GetSection(JwtOptions.SectionKey)
                    .Bind(options.TokenValidationParameters);

                // Establecer la llave para validación de la firma.
                // CORRECCIÓN: Se agrega ?? para evitar el error CS8604 (referencia nula)
                string SecurityKey = builder.Configuration
                    .GetSection(JwtOptions.SectionKey)[nameof(JwtOptions.SecurityKey)]
                    ?? "KeyMustBeAtLeast32CharactersLong";

                byte[] SecurityKeyBytes = Encoding.UTF8.GetBytes(SecurityKey);
                options.TokenValidationParameters.IssuerSigningKey =
                    new SymmetricSecurityKey(SecurityKeyBytes);
            });

        builder.Services.AddAuthorization();

        return builder.Build();
    }

    public static WebApplication ConfigureWebApplication(
        this WebApplication app)
    {
        app.UseExceptionHandler(builder => { });

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapNorthWindSalesEndpoints();
        return app;
    }
}