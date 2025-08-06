using MyShopSite.Infostructure.BusinessService;

namespace MyShopSite.Startup
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddBusinessServiceModule();
        }
        //public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddDbContext<>(options =>
        //        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        //}

        public static void AddCoresPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalNetwork", policy =>
                {
                    policy.WithOrigins("")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }
    }
}
