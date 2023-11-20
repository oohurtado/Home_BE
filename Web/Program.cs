
using Hangfire;
using Hangfire.SqlServer;
using Home.Source.BusinessLayer;
using Home.Source.Data;
using Home.Source.Data.Infrastructure;
using Home.Source.Data.Repositories;
using Home.Source.Hubs;
using Home.Source.Models.Entities;
using Home.Source.Services.Github;
using Home.Source.Services.Message;
using Home.Source.Services.Time;
using Home.Source.Services.WorkerX;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace Home
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            ConfigureApp(builder);
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // Add services to the container.

            builder.Services.AddControllers()
                //.AddJsonOptions(p => p.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            //builder.Services.AddHostedService<BackgroundWorkerService1>();
            builder.Services.AddHostedService<BackgroundWorkerService2>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors();

            builder.Services
                .AddIdentity<User, IdentityRole>(p =>
                {
                    p.User.RequireUniqueEmail = true;
                    p.Password.RequireDigit = false;
                    p.Password.RequiredUniqueChars = 0;
                    p.Password.RequireLowercase = false;
                    p.Password.RequireNonAlphanumeric = false;
                    p.Password.RequireUppercase = false;
                    p.Password.RequiredLength = 3;
                })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddDbContext<DatabaseContext>(p =>
            {
                p.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            _ = builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })                
                .AddJwtBearer(options =>
                {
                    //options.Authority = "http://localhost:4200";
                    //options.Events = new JwtBearerEvents
                    //{
                    //    OnMessageReceived = context =>
                    //    {
                    //        var accessToken = context.Request.Query["access_token"];

                    //        // If the request is for our hub...
                    //        var path = context.HttpContext.Request.Path;
                    //        if (!string.IsNullOrEmpty(accessToken) &&
                    //            (path.StartsWithSegments("/hubs/test1")))
                    //        {
                    //            // Read the token out of the query string
                    //            context.Token = accessToken;
                    //        }
                    //        return Task.CompletedTask;
                    //    }
                    //};
                    options.IncludeErrorDetails = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
                        ClockSkew = TimeSpan.Zero,
                    };
                }
            );

            // output cache
            builder.Services.AddOutputCache(o =>
            {
                o.AddPolicy("Policy-People-Get", b => b.Expire(TimeSpan.FromSeconds(120)).Tag("People-Get"));
            });

            // httpclient
            builder.Services.AddHttpClient<IGitHubService, GitHubService>();

            builder.Services.AddSignalR(p => p.EnableDetailedErrors = true);
            //builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();

            // repositories
            builder.Services.AddScoped<IAspNetRepository, AspNetRepository>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();
            builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();

            // services
            builder.Services.AddScoped<ITimeService, TimeService>();
            builder.Services.AddScoped<IMessageService, EmailService>();
            builder.Services.AddScoped<IMessageService, SMSService>();
            builder.Services.AddScoped<IGitHubService, GitHubPollyService>();

            // layers
            builder.Services.AddScoped<ConfigurationLayer>();
            builder.Services.AddScoped<SeedLayer>();
            builder.Services.AddScoped<UserLayer>();
            builder.Services.AddScoped<PeopleLayer>();

            //// hangfire
            //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            //// hangfire ... as client
            //builder.Services.AddHangfire(configuration => configuration
            //    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            //    .UseSimpleAssemblyNameTypeSerializer()
            //    .UseRecommendedSerializerSettings()
            //    .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            //    {
            //        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            //        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            //        QueuePollInterval = TimeSpan.Zero,
            //        UseRecommendedIsolationLevel = true,
            //        DisableGlobalLocks = true,
            //    }));
            //// hangfire ... as server
            //builder.Services.AddHangfireServer(/*options => options.SchedulePollingInterval = TimeSpan.FromSeconds(1)*/);
        }

        private static void ConfigureApp(WebApplicationBuilder builder)
        {
            var app = builder.Build();
            InitSeed(app.Services);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            // output cache
            app.UseOutputCache();

            //// hangfire
            //app.UseHangfireDashboard();
            //RecurringJob.AddOrUpdate<ITimeService>("print-time", service => service.PrintTime(), Cron.Minutely);
            ////RecurringJob.AddOrUpdate<ITimeService>("print-time", service => service.PrintTime(), "*/1 * * * *");

            app.UseCors(builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200") //*
            );

            app.MapControllers();

            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var message = new List<string>();
                        var error = string.Empty;

                        // CUSTOM_ERROR: users can see
                        // SERVER_ERROR: developers can see

                        if (contextFeature.Error.Message.Contains("CUSTOM_ERROR"))
                        {
                            error = contextFeature.Error.Message.Replace("CUSTOM_ERROR - ", "");
                        }
                        else
                        {
                            var tmp = string.Empty;
                            if (contextFeature.Error.Message.Contains("SERVER_ERROR"))
                            {
                                tmp = contextFeature.Error.Message.Replace("SERVER_ERROR - ", "");
                            }
                            else
                            {
                                tmp = contextFeature.Error.Message;

                                if (contextFeature.Error.InnerException != null)
                                {
                                    tmp = contextFeature.Error.InnerException.Message;
                                }
                            }
                            // TODO: log 'tmp' error for devs

                            error = $"Internal server error. Please try again later. {tmp}";
                        }

                        message.Add(error);
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(message));
                    }
                });
            });

            app.MapHub<SignalR_Test1_Hub>("hubs/test1");
            app.MapHub<SignalR_Test2_Hub>("hubs/test2");

            app.Run();
        }

        private static void InitSeed(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var init = scope.ServiceProvider.GetService<SeedLayer>();
            init?.InitAsync().Wait();
        }
    }

    //public class UserIdProvider : IUserIdProvider
    //{
    //    public string? GetUserId(HubConnectionContext connection)
    //    {
    //        ///var val = connection.User?.Claims.FirstOrDefault(x => x.Type == "user_id")?.Value;
    //        /// var val = connection.User?.Claims.Where(p => p.Value)
    //        return val;
    //    }
    //}

    //public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    //{
    //    public void PostConfigure(string? name, JwtBearerOptions options)
    //    {
    //        var originalOnMessageReceived = options.Events.OnMessageReceived;
    //        options.Events.OnMessageReceived = async context =>
    //        {
    //            await originalOnMessageReceived(context);

    //            if (string.IsNullOrEmpty(context.Token))
    //            {
    //                var accessToken = context.Request.Query["access_token"];
    //                var path = context.HttpContext.Request.Path;

    //                if (!string.IsNullOrEmpty(accessToken) &&
    //                    path.StartsWithSegments("/hubs"))
    //                {
    //                    context.Token = accessToken;
    //                }
    //            }
    //        };
    //    }
    //}
}