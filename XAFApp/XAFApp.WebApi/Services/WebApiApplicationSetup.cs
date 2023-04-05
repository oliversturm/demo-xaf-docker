using DevExpress.ExpressApp;
using DevExpress.ExpressApp.AspNetCore;
using DevExpress.ExpressApp.AspNetCore.WebApi;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFApp.WebApi.Core;

public class WebApiApplicationSetup : IWebApiApplicationSetup {
  public void SetupApplication(AspNetCoreApplication application) {
    application.Modules.Add(new XAFApp.Module.XAFAppModule());

#if DEBUG
        if(System.Diagnostics.Debugger.IsAttached) {
            application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
        }
#endif
    application.DatabaseVersionMismatch += (s, e) => {
      e.Updater.Update();
      e.Handled = true;
    };
  }
}
