using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Models
{
   
    public class CollectionRevenue
    {
        public long id { get; set; }
        public string nifap { get; set; }
        public string nome { get; set; }
        public string trans_msg { get; set; }       
        public double current_balance { get; set; }
        public int quantity { get; set; }
        public int trans_method_id { get; set; }

        public decimal unit_amount { get; set; }

    }
    public class BillingUpdateAddressRequest
    {
        public long id { get; set; }

        public int? entity_id { get; set; }
        public string? address_line1 { get; set; }
        public string? address_line2 { get; set; }
    //    public string? city { get; set; }
    //    public string? state { get; set; }
    //    public string? country { get; set; }
      //  public DateTime? deleted_at { get; set; }
        public int? pin { get; set; }
        public int? distrito_id { get; set; }
        public int? country_id { get; set; }
        public int? fregusia_id { get; set; }
    }
    public class BillingInsertAddressRequest
    {

        public int entity_id { get; set; }
        public string? address_line1 { get; set; }
        public string? address_line2 { get; set; }
      
        public DateTime? deleted_at { get; set; }
        public int? pin { get; set; }

    }
    public class UpdateBilling
    {
        public int entityId { get; set; }
        public int billingId { get; set; }

    }
    public class UpdatePaymentMethod
    {
        public long transId { get; set; }
        public int paymentMethodId { get; set; }

    }
    public class FilterCollectionRevenue
    {
        public Dictionary<string, string> Filters { get; set; }
        public string SortBy { get; set; }
        public bool? IsSortTypeDESC { get; set; }
        public bool? IsPagination { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
