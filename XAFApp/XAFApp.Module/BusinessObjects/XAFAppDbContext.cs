using DevExpress.ExpressApp.EFCore.Updating;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.DesignTime;
using XAFApp.Module.BusinessObjects;

namespace XAFApp.Module.BusinessObjects;

// This code allows our Model Editor to get relevant EF Core metadata at design time.
// For details, please refer to https://supportcenter.devexpress.com/ticket/details/t933891.
public class XAFAppContextInitializer : DbContextTypesInfoInitializerBase {
  protected override DbContext CreateDbContext() {
    var optionsBuilder = new DbContextOptionsBuilder<XAFAppEFCoreDbContext>()
            .UseSqlServer(";")
            .UseChangeTrackingProxies()
            .UseObjectSpaceLinkProxies();
    return new XAFAppEFCoreDbContext(optionsBuilder.Options);
  }
}
//This factory creates DbContext for design-time services. For example, it is required for database migration.
public class XAFAppDesignTimeDbContextFactory : IDesignTimeDbContextFactory<XAFAppEFCoreDbContext> {
  public XAFAppEFCoreDbContext CreateDbContext(string[] args) {
    throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
    //var optionsBuilder = new DbContextOptionsBuilder<XAFAppEFCoreDbContext>();
    //optionsBuilder.UseSqlServer("Integrated Security=SSPI;Pooling=false;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=XAFApp");
    //optionsBuilder.UseChangeTrackingProxies();
    //optionsBuilder.UseObjectSpaceLinkProxies();
    //return new XAFAppEFCoreDbContext(optionsBuilder.Options);
  }
}
[TypesInfoInitializer(typeof(XAFAppContextInitializer))]
public class XAFAppEFCoreDbContext : DbContext {
  public XAFAppEFCoreDbContext(DbContextOptions<XAFAppEFCoreDbContext> options) : base(options) {
  }
  //public DbSet<ModuleInfo> ModulesInfo { get; set; }

  public DbSet<SaleProduct> SaleProducts { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    base.OnModelCreating(modelBuilder);
    modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
  }
}
