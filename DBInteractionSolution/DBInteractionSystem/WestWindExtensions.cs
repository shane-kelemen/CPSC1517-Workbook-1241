using DBInteractionSystem.DAL;
using DBInteractionSystem.BLL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DBInteractionSystem
{
    public static class WestWindExtensions
    {
        // This class can hold a set of extension methods
        // This sample will create an extension method that will add services to 
        //      IServiceCollection
        // This method will be called from a separate project to gain data from the 
        //      WestWind database
        // In this demonstration, the call will be completed from the DBInteractionWebApp 
        //      Program.cs file
        // This method will do two things
        // 1) Register the context connection string
        // 2) Register ALL services that we create within the BLL classes
        public static void WestWindExtensionServices(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<WestWindContext>(options);

            services.AddTransient<RegionServices>(serviceProvider =>
            {
                var context = serviceProvider.GetService<WestWindContext>();
                return new RegionServices(context);
            });
        }
    }
}
