using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VotingSystem.Application;
using VotingSystem.Application;
using VotingSystem.Database;

namespace VotingSystem.Ui
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Cookie")
                .AddCookie("Cookie");

            services.AddDbContext<AppDbContext>(options => {
                options.UseInMemoryDatabase("Database");
            });

            services.AddSingleton<IVotingPollFactory, VotingPollFactory>();
            services.AddSingleton<ICounterManager, CounterManager>();
            services.AddScoped<VotingInteractor>();
            services.AddScoped<StatisticsInteractor>();
            services.AddScoped<VotingPollInteractor>();
            services.AddScoped<IVotingSystemPersistance, VotingSystemPersistance>();
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
