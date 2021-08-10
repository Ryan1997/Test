using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using JokesWebApp.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using System.IdentityModel.Tokens.Jwt;

namespace JokesWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();
            //  services.AddRazorPages();

            JwtSecurityTokenHandler.DefaultMapInboundClaims = true;

            services.AddAuthentication(options => {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc"; 
                options.DefaultAuthenticateScheme = "oidc";
                options.DefaultSignOutScheme = "oidc";

            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    //options.Authority = "https://120.78.208.87:443";
                    options.Authority = "https://localhost";

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                });

            services.AddAuthentication().AddMicrosoftAccount(microsoftOptions =>
            {
                // 客户端ID
                microsoftOptions.ClientId = "8d025ec8-8526-48c1-9c8b-c8e077875efc";
                // 授权密码的值
                microsoftOptions.ClientSecret = "Q-nQjDW-Hi_rFjit._XaH~prB3HL-t0t58";
                microsoftOptions.CallbackPath = "/signin-microsoft";
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }*/
            // adding
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
