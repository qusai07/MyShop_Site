using MyShopSite.Repo.Implementations;
using MyShopSite.Repo.Interfaces;

namespace MyShopSite.Infostructure.BusinessService
{
    public static class BusinessServiceModule
    {
        public static void AddBusinessServiceModule(this IServiceCollection Services)
        {
            Services.AddScoped<IDataService, DataService>();

        }
    }
}
