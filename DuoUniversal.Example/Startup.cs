// SPDX-FileCopyrightText: 2022 Cisco Systems, Inc. and/or its affiliates
//
// SPDX-License-Identifier: BSD-3-Clause

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DuoUniversal.Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions<Config>()
                .Configure((Config config) =>
                {
                    config.ClientId = Configuration["Client ID"];
                    config.ClientSecret = Configuration["Client Secret"];
                    config.ApiHost = Configuration["Api Host"];
                    config.RedirectUri = Configuration["Redirect Uri"];
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();

            // This is one possible way to make a Duo client factory available, there are many other options.
            services.AddSingleton<ClientBuilder>(provider =>
            {
                var (clientId, clientSecret, apiHost, redirectUri) = provider.GetRequiredService<IOptions<Config>>().Value;
                return new(clientId, clientSecret, apiHost, redirectUri);
            });
            services.AddTransient<Client>(provider=>
            {
                var builder = provider.GetRequiredService<ClientBuilder>();
                return builder.Build();
            });

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;

            }
            );
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
