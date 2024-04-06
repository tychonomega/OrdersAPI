using App.Configuration;
using App.Data;
using App.Middlewares;
using App.Services;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace OrdersAPI.Main
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.ConfigureAppConfiguration((configBuilder) =>
            {
                configBuilder.Sources.Clear();
                DotEnv.Load();
                configBuilder.AddEnvironmentVariables();
            });

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.AddServerHeader = false;
            });

            // Add services to the container.
            builder.Services.AddScoped<IMessageService, MessageService>();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(
                        builder.Configuration.GetValue<string>("CLIENT_ORIGIN_URL"))
                        .WithHeaders(new string[] {
                HeaderNames.ContentType,
                HeaderNames.Authorization,
                        })
                        .WithMethods("GET")
                        .SetPreflightMaxAge(TimeSpan.FromSeconds(86400));
                });
            });

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddControllers();

            builder.Host.ConfigureServices((services) =>
            {
                services.Configure<MongoConnectionSettings>(options =>
                {
                    options.ConnectionString = builder.Configuration.GetSection("MongoDB:ConnectionString").Value;
                    options.Database = builder.Configuration.GetSection("MongoDB:Database").Value;
                });

                services.AddTransient<IOrderContext, OrderContext>();
                services.AddScoped<IOrderRepository, OrderRepository>();
                
                services.AddSwaggerDocumentation();


                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        var audience =
                              builder.Configuration.GetValue<string>("AUTH0_AUDIENCE");

                        options.Authority =
                              $"https://{builder.Configuration.GetValue<string>("AUTH0_DOMAIN")}/";
                        options.Audience = audience;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true
                        };
                    });

            }
            );

            var app = builder.Build();


            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerDocumentation();
            }


            var requiredVars =
                new string[] {
          "PORT",
          "CLIENT_ORIGIN_URL",
          "AUTH0_DOMAIN",
          "AUTH0_AUDIENCE",
                };

            foreach (var key in requiredVars)
            {
                var value = app.Configuration.GetValue<string>(key);

                if (value == "" || value == null)
                {
                    throw new Exception($"Config variable missing: {key}.");
                }
            }

            app.Urls.Add(
                $"http://+:{app.Configuration.GetValue<string>("PORT")}");

            app.UseErrorHandler();
            app.UseSecureHeaders();
            app.MapControllers();
            app.UseCors();

            string testMode = builder.Configuration.GetSection("TEST_MODE").Value;

            if (testMode == "true")
            {
                app.UseAuthenticatedTestRequest();
            }

            app.UseAuthentication();
            app.UseAuthorization();



            app.Run();
        }
    }
}