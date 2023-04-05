using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.Persistent.Base;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.EntityFrameworkCore;
using XAFApp.Blazor.Server.Services;
using DevExpress.ExpressApp.Core;
using Antlr4.StringTemplate;

namespace XAFApp.Blazor.Server;

public class Startup {
  public Startup(IConfiguration configuration) {
    Configuration = configuration;
  }

  public IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container.
  // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
  public void ConfigureServices(IServiceCollection services) {
    services.AddSingleton(typeof(Microsoft.AspNetCore.SignalR.HubConnectionHandler<>), typeof(ProxyHubConnectionHandler<>));

    services.AddRazorPages();
    services.AddServerSideBlazor();
    services.AddHttpContextAccessor();
    services.AddScoped<CircuitHandler, CircuitHandlerProxy>();
    services.AddXaf(Configuration, builder => {
      builder.UseApplication<XAFAppBlazorApplication>();
      builder.Modules
          .Add<XAFApp.Module.XAFAppModule>()
        .Add<XAFAppBlazorModule>();
      builder.ObjectSpaceProviders
          .AddEFCore().WithDbContext<XAFApp.Module.BusinessObjects.XAFAppEFCoreDbContext>((serviceProvider, options) => {
            // Uncomment this code to use an in-memory database. This database is recreated each time the server starts. With the in-memory database, you don't need to make a migration when the data model is changed.
            // Do not use this code in production environment to avoid data loss.
            // We recommend that you refer to the following help topic before you use an in-memory database: https://docs.microsoft.com/en-us/ef/core/testing/in-memory
            //options.UseInMemoryDatabase("InMemory");
            var connectionStringTemplate = new Template(Configuration.GetConnectionString("ConnectionString"));
            connectionStringTemplate.Add("SQL_DBNAME", System.Environment.GetEnvironmentVariable("SQL_DBNAME"));
            connectionStringTemplate.Add("SQL_SA_PASSWD", System.Environment.GetEnvironmentVariable("SQL_SA_PASSWD"));
            options.UseSqlServer(connectionStringTemplate.Render());
            Console.WriteLine("Used SQL Server with connection string: " + connectionStringTemplate.Render());
            options.UseChangeTrackingProxies();
            options.UseObjectSpaceLinkProxies();
            options.UseLazyLoadingProxies();
          })
          .AddNonPersistent();
    });
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
    if (env.IsDevelopment()) {
      app.UseDeveloperExceptionPage();
    }
    else {
      app.UseExceptionHandler("/Error");
      // The default HSTS value is 30 days. To change this for production scenarios, see: https://aka.ms/aspnetcore-hsts.
      app.UseHsts();
    }
    //app.UseHttpsRedirection();
    app.UseRequestLocalization();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseXaf();
    app.UseEndpoints(endpoints => {
      endpoints.MapXafEndpoints();
      endpoints.MapBlazorHub();
      endpoints.MapFallbackToPage("/_Host");
      endpoints.MapControllers();
    });
  }
}
