using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Models
{
    public class AllTransaction
    {
      //  [NotMapped]
      //  public List<IssueDocumentDetails> issueDocumentDetail { get; set; }
        [NotMapped]
        public List<AllTransactions> allTransaction { get; set; }
    }
    public class AllTransactionInvoice
    {
         [NotMapped]
          public AllTransactions allTransaction { get; set; }

     /*   public long id { get; set; }
        public long parent_id { get; set; }
        public long billing_id { get; set; }
        public long? vat_no { get; set; }

        public int entity_id { get; set; }
        public long entityacc_Id { get; set; }
        public long cashier_Id { get; set; }

        public long trans_no { get; set; }

        public string trans_type { get; set; }
        public string trans_msg { get; set; }

        public int trans_method_id { get; set; }
        public double? total_cr { get; set; }
        public double? total_dr { get; set; }
        public DateTime trans_date { get; set; }
        public string base_currency { get; set; }
        public string trans_currency { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime? deleted_at { get; set; }

        public string Comment { get; set; }
        public decimal captive_balance { get; set; }
        public decimal? vat_rate { get; set; }

        public double current_balance { get; set; }
     
        public string bank { get; set; }

        public string useful_cashier { get; set; }
        [NotMapped]

        public string cashier_place { get; set; }
        [NotMapped]

        public string nome { get; set; }
        public string despagerecord { get; set; }

        public decimal accumulated_previous { get; set; }

        public decimal bead_no { get; set; }*/
        [NotMapped]
        public IssueDocumentDetails issueDocumentDetail { get; set; }
        [NotMapped]
        public BusEntidade busEntidadeDetail { get; set; }
        
    }
    public class FilterAllTransaction
    {
        public Dictionary<string, string> Filters { get; set; }
        public string SortBy { get; set; }
        public bool? IsSortTypeDESC { get; set; }
        public bool? IsPagination { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }

    public class UpdateInvoice
    {
        public string? date { get; set; }
        public string? flag { get; set; }
        public string? document_type { get; set; }
        public string? document_url { get; set; }
        public decimal? document_size { get; set; }
        public int? status { get; set; }
        public int? remove_tax { get; set; }

        public int? cancel_invoice { get; set; }
        public string? issue_date_time { get; set; }
       // public DateTime? deadline_date { get; set; }
        public string? extra_text { get; set; }
        public string? no { get; set; }
        public int? pag_limit { get; set; }

        public string? iva { get; set; }
        public double? amount_paid { get; set; }


    }

}
