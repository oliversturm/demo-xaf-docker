using DevExpress.Persistent.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DevExpress.ExpressApp.WebApi.Services;
using Microsoft.AspNetCore.OData;
using DevExpress.ExpressApp.Core;
using XAFApp.WebApi.Core;
using DevExpress.ExpressApp.AspNetCore.WebApi;
using Antlr4.StringTemplate;

namespace XAFApp.WebApi;

public class Startup {
  public Startup(IConfiguration configuration) {
    Configuration = configuration;
  }

  public IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container.
  // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
  public void ConfigureServices(IServiceCollection services) {
    services
        .AddScoped<IObjectSpaceProviderFactory, ObjectSpaceProviderFactory>()
        .AddSingleton<IWebApiApplicationSetup, WebApiApplicationSetup>();

    services.AddDbContextFactory<XAFApp.Module.BusinessObjects.XAFAppEFCoreDbContext>((serviceProvider, options) => {
      // Uncomment this code to use an in-memory database. This database is recreated each time the server starts. With the in-memory database, you don't need to make a migration when the data model is changed.
      // Do not use this code in production environment to avoid data loss.
      // We recommend that you refer to the following help topic before you use an in-memory database: https://docs.microsoft.com/en-us/ef/core/testing/in-memory
      //options.UseInMemoryDatabase("InMemory");
      var connectionStringTemplate = new Template(Configuration.GetConnectionString("ConnectionString"));
      connectionStringTemplate.Add("SQL_DBNAME", System.Environment.GetEnvironmentVariable("SQL_DBNAME"));
      connectionStringTemplate.Add("SQL_SA_PASSWD", System.Environment.GetEnvironmentVariable("SQL_SA_PASSWD"));
      options.UseSqlServer(connectionStringTemplate.Render());
      options.UseChangeTrackingProxies();
      options.UseObjectSpaceLinkProxies();
      options.UseLazyLoadingProxies();
    }, ServiceLifetime.Scoped);

    services
        .AddXafWebApi(Configuration, options => {
          // Make your business objects available in the Web API and generate the GET, POST, PUT, and DELETE HTTP methods for it.
          // options.BusinessObject<YourBusinessObject>();
          options.BusinessObject<XAFApp.Module.BusinessObjects.SaleProduct>();
        });
    services
        .AddControllers()
        .AddOData((options, serviceProvider) => {
          options
                  .AddRouteComponents("api/odata", new EdmModelBuilder(serviceProvider).GetEdmModel())
                  .EnableQueryFeatures(100);
        });

    services.AddSwaggerGen(c => {
      c.EnableAnnotations();
      c.SwaggerDoc("v1", new OpenApiInfo
      {
        Title = "XAFApp API",
        Version = "v1",
        Description = @"Use AddXafWebApi(options) in the XAFApp.WebApi\Startup.cs file to make Business Objects available in the Web API."
      });
    });
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
    if (env.IsDevelopment()) {
      app.UseDeveloperExceptionPage();
      app.UseSwagger();
      app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "XAFApp WebApi v1");
      });
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
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints(endpoints => {
      endpoints.MapControllers();
    });
  }
}
