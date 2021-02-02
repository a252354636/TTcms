﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using TTcms.Identity.API.Data;

namespace TTcms.Identity.API
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        public static void Main(string[] args)
        {
            Log.Information("Configuring web host ({ApplicationContext})...", AppName);
            var configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(configuration);

            Log.Information("Configuring web host ({ApplicationContext})...", AppName);
            var host = BuildWebHost(configuration, args);
            Log.Information("Applying migrations ({ApplicationContext})...", AppName);


            //host.MigrateDbContext<PersistedGrantDbContext>((_, __) => { })
            //    .MigrateDbContext<ApplicationDbContext>((context, services) =>
            //    {
            //        var env = services.GetService<IHostingEnvironment>();
            //        var logger = services.GetService<ILogger<ApplicationDbContextSeed>>();
            //        var settings = services.GetService<IOptions<AppSettings>>();

            //        new ApplicationDbContextSeed()
            //            .SeedAsync(context, env, logger, settings)
            //            .Wait();
            //    })
            //    .MigrateDbContext<ConfigurationDbContext>((context, services) =>
            //    {
            //        new ConfigurationDbContextSeed()
            //            .SeedAsync(context, configuration)
            //            .Wait();
            //    });


            Log.Information("Starting web host ({ApplicationContext})...", AppName);
            host.Run();
        }

        private static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
             WebHost.CreateDefaultBuilder(args)
                 .CaptureStartupErrors(false)
                 .UseStartup<Startup>()
   
                 .UseContentRoot(Directory.GetCurrentDirectory())
                 .UseConfiguration(configuration)
                 .UseSerilog()
                 .Build();

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            //if (config.GetValue<bool>("UseVault", false))
            //{
            //    builder.AddAzureKeyVault(
            //        $"https://{config["Vault:Name"]}.vault.azure.net/",
            //        config["Vault:ClientId"],
            //        config["Vault:ClientSecret"]);
            //}

            return builder.Build();
        }
        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            var logstashUrl = configuration["Serilog:LogstashgUrl"];
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(string.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .WriteTo.Http(string.IsNullOrWhiteSpace(logstashUrl) ? "http://localhost:8080" : logstashUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }
    }
}