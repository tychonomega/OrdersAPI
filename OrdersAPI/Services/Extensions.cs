using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace App.Services
{
    public static class Extensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OrdersAPI v1.0", Version = "v1" });

                c.AddSecurityDefinition(
                    "oauth2",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri("https://dev-hloe0m6fbmv25qkt.us.auth0.com/authorize"),
                                TokenUrl = new Uri("https://dev-hloe0m6fbmv25qkt.us.auth0.com/oauth/token"),
                                Scopes = new Dictionary<string, string> {
                                    {"openid", "openid"},
                                    {"name", "name"},
                                    {"email", "email"},
                                },
                                
                            }
                        }
                    });


                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "oauth2", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                    });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1.0");
                c.DocumentTitle = "Title Documentation";
                c.DocExpansion(DocExpansion.None);
                c.RoutePrefix = string.Empty;
                c.OAuthClientId("Hk3siMnbTF0usKIHAO9EDEBZ1GDSJywN");
                c.OAuthClientSecret("0V-1cJSIjnnOT30RJNdrOjUB4XaKH6baH6SjFLSiHQtuQsrsjT0rzigDU7wiUDZ1");
                c.OAuthAppName("OrdersAPI");
                c.OAuthScopeSeparator(",");
                //c.UseRequestInterceptor("(req) => { if (req.url.endsWith('oauth/token') && req.body) req.body += '&audience=https%3A%2F%2Forders.example.com'; return req; }");
                c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>() { { "audience", "https://orders.example.com" } });

            });

            return app;
        }
    }
}
