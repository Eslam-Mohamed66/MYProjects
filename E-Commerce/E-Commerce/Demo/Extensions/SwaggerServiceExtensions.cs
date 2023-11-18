using Microsoft.OpenApi.Models;

namespace Demo.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiDemo", Version = "v1" });

                var secuirtySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer schema",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "bearer"
                    }
                };
                c.AddSecurityDefinition("bearer", secuirtySchema);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {secuirtySchema, new[] {"bearer"} }
                };

                c.AddSecurityRequirement(securityRequirement);
            });

            return services;
        }
    }
}
