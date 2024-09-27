using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Animle.services.Quartz;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Animle.SignalR;
using Animle.Interfaces;
using Animle.services.Token;
using Animle.Services;

namespace Animle.services
{
    public static class BuilderServiceConfiguration
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
              .Build();
            builder.Services.AddRateLimiter(opt =>
            {
                   opt.AddFixedWindowLimiter(policyName: "fixed", options =>
                    {
                        options.PermitLimit = 6;
                        options.Window = TimeSpan.FromSeconds(5);
                        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                        options.QueueLimit = 10;
                    });
        });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.RegisterCronJobs();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
         
            builder.Services.AddDbContext<AnimleDbContext>(options =>
            {
            string dbConnect = configuration.GetSection("AppSettings:DbConnection").Value;
            options.UseMySql(dbConnect, ServerVersion.AutoDetect(dbConnect));

        });
          
            builder.Services.AddSignalR();
            builder.Services.AddScoped<CustomAuthorizationFilter>();

            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddSingleton<SignalrAnimeService>();
            builder.Services.AddSingleton<IRequestCacheManager, RequestCacheManager>();
            builder.Services.AddScoped<IAnimeService, AnimeService>();
            builder.Services.AddScoped<IQuizService, QuizService>();
            builder.Services.AddScoped<IUserService, UserService>();


            builder.Services.AddSingleton<TokenService>(provider =>
            {
                var secretKey = configuration.GetSection("AppSettings:SecretKey").Value;

                return new TokenService(secretKey);
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200") 
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

        }
    }
}
