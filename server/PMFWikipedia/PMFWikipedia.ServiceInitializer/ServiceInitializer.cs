using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PMFWikipedia.Common;
using PMFWikipedia.ImplementationsDAL;
using PMFWikipedia.ImplementationsDAL.PMFWikipedia.ImplementationsDAL;
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

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services) 
        {
            services.InitializeDAL();
            return services;
        }
    }

}