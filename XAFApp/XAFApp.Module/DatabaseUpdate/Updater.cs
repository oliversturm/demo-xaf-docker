using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.EF;
using DevExpress.Persistent.BaseImpl.EF;
using XAFApp.Module.BusinessObjects;

namespace XAFApp.Module.DatabaseUpdate;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
  public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
      base(objectSpace, currentDBVersion) {
    Console.WriteLine("Updater ctor, currentDBVersion: " + currentDBVersion);
  }
  public override void UpdateDatabaseAfterUpdateSchema() {
    base.UpdateDatabaseAfterUpdateSchema();

    var rubberChicken = ObjectSpace.FirstOrDefault<SaleProduct>(p => p.Name == "Rubber Chicken");
    if (rubberChicken == null) {
      // we assume that the demo data doesn't exist yet
      rubberChicken = ObjectSpace.CreateObject<SaleProduct>();
      rubberChicken.Name = "Rubber Chicken";
      rubberChicken.Price = 13.99m;
      var pulley = ObjectSpace.CreateObject<SaleProduct>();
      pulley.Name = "Pulley";
      pulley.Price = 3.99m;
      var enterprise = ObjectSpace.CreateObject<SaleProduct>();
      enterprise.Name = "Starship Enterprise";
      enterprise.Price = 149999999.99m;
      var lostArk = ObjectSpace.CreateObject<SaleProduct>();
      lostArk.Name = "The Lost Ark";
      lostArk.Price = 1000000000000m;
    }

    ObjectSpace.CommitChanges();
  }
  public override void UpdateDatabaseBeforeUpdateSchema() {
    base.UpdateDatabaseBeforeUpdateSchema();
  }
}
