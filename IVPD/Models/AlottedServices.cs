using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    [Table("AlottedServices")]

    public class AlottedServices
    {
        [Key]
        public long id { get; set; }
        public long service_id { get; set; }
        public long? trans_id { get; set; }

        public long? invoice_id { get; set; }
        public string? local { get; set; }

        public long? cashier_id { get; set; }
        public int? tax_id { get; set; }

        public int entity_id { get; set; }
        public int unit { get; set; }
        public double? valor { get; set; }
        public double? valor_cativo { get; set; }
        public double? service_tax { get; set; }

        public decimal? tax_percent { get; set; }
        public string tax_type { get; set; }

        public DateTime? date { get; set; }
        public int? is_invoiced { get; set; }
        [NotMapped]
        public BusEntidade busEntidadeDetail { get; set; }
        [NotMapped]
        public LIBTESOUR_TSARTIF service { get; set; }
        [NotMapped]
        public double? service_amount { get; set; }
        public double? per_services_amount { get; set; }

        [NotMapped]
        public double? complete_services_amount { get; set; }
       
    }
    public class FilterAlottedServices
    {
        public Dictionary<string, string> Filters { get; set; }
        public string SortBy { get; set; }
        public bool? IsSortTypeDESC { get; set; }
        public bool? IsPagination { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
    public class FilterAlottedServicesByInvoice
    {
        public long invoiceId { get; set; }
        public Dictionary<string, string> Filters { get; set; }
        public string SortBy { get; set; }
        public bool? IsSortTypeDESC { get; set; }
        public bool? IsPagination { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
    public class AlottedServicesCreateRequest
    {
        [NotMapped]
        public List<AlottedServices> services { get; set; }
        public int? entity_id { get; set; }
        public int future_payment { get; set; }
        public double? total_amount { get; set; }
        public double? amount_to_be_used { get; set; }

        public string? deadline_date { get; set; }
        public string? local { get; set; }
        
        public double? per_services_amount { get; set; }

        public double? total_services_amount { get; set; }
        public double? total_service_tax { get; set; }
        public double? total_valor_cativo_tax { get; set; }


    }
}
