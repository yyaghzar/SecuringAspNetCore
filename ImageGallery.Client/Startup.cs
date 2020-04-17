using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace ImageGallery.Client
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                 .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

            // create an HttpClient used for accessing the API
            services.AddHttpClient("APIClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:44366/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            });

            services.AddHttpClient("IDPClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:44337/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            });

            services.AddAuthentication(options =>
            {
                // If you have many application. We should choose a better name for this.
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // this will have to match the scheme that we used to configure OpenIdConnect
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

            })
                // this configures a cookie handler.  Enables application to use cookie-based authentication for that default scheme
                // Once the Identity token is validated and transformed to a claims identity it will be stored in an encrypted cookie
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    //options.AccessDeniedPath = "/Authorization/AccessDenied";
                })

                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                 {
                     options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                     options.Authority = "https://localhost:44337/";
                     options.ClientId = "ImageGalleryClient";
                     options.ClientSecret = "secret";
                     options.ResponseType = "code";
                     options.UsePkce = true;
                     options.Scope.Add("address");
                     options.Scope.Add("roles");
                     // Add Claim to ClaimsIdentity
                     options.ClaimActions.MapUniqueJsonKey("address", "address");
                     options.ClaimActions.MapUniqueJsonKey("role", "role");
                     // Include claims and make sure they aren't filtered out
                     options.ClaimActions.Remove("nbf");

                     // Remove Claims we don't need
                     options.ClaimActions.DeleteClaim("idp");



                     #region this is needed in case we need to change the redirect ui 
                     //In case we need to change signin from the default path "/signin-oidc"
                     //options.CallbackPath = new PathString("...");
                     //In case we need to change signout from the default path "/signout-callback-oidc"
                     //options.SignedOutCallbackPath = new PathString("...");
                     #endregion
                     #region these are requested by default, so no need for them
                     //options.Scope.Add("openid");
                     //options.Scope.Add("profile");
                     #endregion

                     // This will claims from UserEndpoint
                      options.GetClaimsFromUserInfoEndpoint = true;
                     // Allows the Middleware to save the tokens
                      options.SaveTokens = true;

                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         NameClaimType = JwtClaimTypes.GivenName,
                         RoleClaimType = JwtClaimTypes.Role
                     };

                 });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
                // The default HSTS value is 30 days. You may want to change this for
                // production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Gallery}/{action=Index}/{id?}");
            });
        }
    }
}
