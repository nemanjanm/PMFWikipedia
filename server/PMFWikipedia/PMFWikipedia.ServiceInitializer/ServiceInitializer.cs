using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PMFWikipedia.Common;
using PMFWikipedia.Common.AutoMapper;
using PMFWikipedia.Common.EmailService;
using PMFWikipedia.Common.StorageService;
using PMFWikipedia.ImplementationsBL;
using PMFWikipedia.ImplementationsBL.Helpers;
using PMFWikipedia.ImplementationsDAL;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;
using System.Text;

namespace PMFWikipedia.ServiceInitializer
{
    public static class ServiceInitializer
    {
        private static IServiceCollection InitializeDAL(this IServiceCollection services)
        {
            services.AddDbContext<PMFWikiContext>
            (
                options => options.UseSqlServer(ConfigProvider.ConnectionString)
            );
            services.AddScoped<IUserDAL, UserDAL>();
            services.AddScoped<ISubjectDAL, SubjectDAL>();  
            services.AddScoped<IFavoriteSubjectDAL, FavoriteSubjectDAL>();
            services.AddScoped<IMessageDAL, MessageDAL>();
            services.AddScoped<IChatDAL, ChatDAL>();
            services.AddScoped<IPostDAL, PostDAL>();
            services.AddScoped<INotificationDAL, NotificationDAL>();
            services.AddScoped<ICommentDAL, CommentDAL>();
            services.AddScoped<IKolokvijumDAL, KolokvijumDAL>();
            services.AddScoped<IKolokvijumResenjeDAL, KolokvijumResenjeDAL>();
            services.AddScoped<IIspitDAL, IspitDAL>();
            services.AddScoped<IIspitResenjeDAL, IspitResenjeDAL>();
            services.AddScoped<IProgramDAL, ProgramDAL>();
            services.AddScoped<IEditedPostDAL, EditedPostDAL>();
            return services;
        }

        private static IServiceCollection initializeBL(this IServiceCollection services)
        {
            services.AddScoped<IUserBL, UserBL>();
            services.AddScoped<IChatBL, ChatBL>();
            services.AddScoped<ISubjectBL, SubjectBL>();
            services.AddScoped<IFavoriteSubjectBL, FavoriteSubjectBL>();
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IMessageBL, MessageBL>();
            services.AddScoped<IPostBL, PostBL>();
            services.AddScoped<INotificationBL, NotificationBL>();
            services.AddScoped<ICommentBL, CommentBL>();
            services.AddScoped<IKolokvijumBL, KolokvijumBL>();
            services.AddScoped<IKolokvijumResenjeBL, KolokvijumResenjeBL>();
            services.AddScoped<IIspitBL, IspitBL>();
            services.AddScoped<IIspitResenjeBL, IspitResenjeBL>();
            return services;
        }

        private static IServiceCollection InitializeCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy(name: "OfficeOrigins",
                policy =>
                {
                    policy.WithOrigins(ConfigProvider.ClientUrl).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                })
            );

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = ConfigProvider.Issuer,
                    ValidAudience = ConfigProvider.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigProvider.JwtKey)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services) 
        {
            services.initializeBL();
            services.InitializeDAL();
            services.InitializeMapper();
            services.InitializeCommonServices();
            services.InitializeCors();
            services.AddHttpContextAccessor();
            return services;
        }

        private static IServiceCollection InitializeMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        private static IServiceCollection InitializeCommonServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStorageService, StorageService>();
            return services;
        }
    }

}