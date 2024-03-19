using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PMFWikipedia.Common;
using PMFWikipedia.ImplementationsBL;
using PMFWikipedia.ImplementationsDAL;
using PMFWikipedia.ImplementationsDAL.PMFWikipedia.ImplementationsDAL;
using PMFWikipedia.InterfacesBL;
using PMFWikipedia.InterfacesDAL;

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
            return services;
        }

        private static IServiceCollection initializeBL(this IServiceCollection services)
        {
            services.AddScoped<IUserBL, UserBL>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services) 
        {
            services.initializeBL();
            services.InitializeDAL();
            return services;
        }
    }

}