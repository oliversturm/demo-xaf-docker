using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using System.ComponentModel;

namespace XAFApp.Module.BusinessObjects {
  [DefaultClassOptions]
  public class SaleProduct : BaseObject {
    public SaleProduct() {
    }

    [DisplayName("Product Name")]
    public virtual string Name { get; set; }
    public virtual decimal? Price { get; set; }
  }
}