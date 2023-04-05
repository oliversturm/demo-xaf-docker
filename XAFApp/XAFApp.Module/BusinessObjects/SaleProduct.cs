using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFApp.Module.BusinessObjects {
  [DefaultClassOptions]
  public class SaleProduct : BaseObject {
    public SaleProduct() {
    }

    public virtual string Name { get; set; }
    public virtual decimal? Price { get; set; }
  }
}