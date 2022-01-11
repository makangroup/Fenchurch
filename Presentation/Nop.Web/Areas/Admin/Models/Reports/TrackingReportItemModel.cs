using Nop.Web.Framework.Models;

namespace Fenchurch.Web.Areas.Admin.Models.Reports
{
    public class TrackingReportItemModel : BaseNopModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TrackingNumber { get; set; }
        public string Date { get; set; }
    }
}