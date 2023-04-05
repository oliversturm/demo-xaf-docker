using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.Security;
using Microsoft.EntityFrameworkCore;

namespace XAFApp.WebApi.Core;

public sealed class ObjectSpaceProviderFactory : IObjectSpaceProviderFactory {
    readonly ITypesInfo typesInfo;
    readonly IDbContextFactory<XAFApp.Module.BusinessObjects.XAFAppEFCoreDbContext> dbFactory;

    public ObjectSpaceProviderFactory(ITypesInfo typesInfo, IDbContextFactory<XAFApp.Module.BusinessObjects.XAFAppEFCoreDbContext> dbFactory) {
        this.typesInfo = typesInfo;
        this.dbFactory = dbFactory;
    }

    IEnumerable<IObjectSpaceProvider> IObjectSpaceProviderFactory.CreateObjectSpaceProviders() {
        yield return new EFCoreObjectSpaceProvider<XAFApp.Module.BusinessObjects.XAFAppEFCoreDbContext>(dbFactory, typesInfo);
        yield return new NonPersistentObjectSpaceProvider(typesInfo, null);
    }
}
