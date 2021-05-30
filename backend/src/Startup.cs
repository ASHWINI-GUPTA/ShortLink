using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShortLink.Repositories;
using ShortLink.Repositories.Interfaces;
using ShortLink.Services;
using ShortLink.Services.Interface;

namespace ShortLink
{
    public class Startup
    {
        private readonly IWebHostEnvironment _hostContext;

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration, IWebHostEnvironment hostContext)
        {
            _configuration = configuration;
            _hostContext = hostContext;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_hostContext.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = _configuration.GetConnectionString("Redis");
                    options.InstanceName = "ShortLink";
                });
            }

            services.AddControllers();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<ILinkService, LinkService>();
            services.AddScoped<ILinkRepository, LinkRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("redirect", "/", new { controller = "Redirect", action = "Get" });
                endpoints.MapControllers();
            });
        }
    }
}
