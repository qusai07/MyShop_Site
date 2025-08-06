namespace MyShopSite.Startup
{
    public static class MiddlewareConfig
    {
        public static void UseGlobalMiddleware(this WebApplication app)
        {
            app.UseCors("AllowLocalNetwork");

            app.Use(async (context, next) =>
            {
                Console.WriteLine($"Request from {context.Connection.RemoteIpAddress} to {context.Request.Path}");
                await next();
            });


            //Type Of Environments
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
