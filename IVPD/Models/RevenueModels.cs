using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IVPD.Models
{
    public class RevenueModels
    {
        [Table("BusEntidadeContacto")]
        public class BusEntidadeContacto
        {
            [Key]

            public int codEntidadeContacto { get; set; }
            public int? codEntidade { get; set; }
            public int? codCaracteristica { get; set; }
            public int? codTipoContacto { get; set; }
            public string? codTipoContacto2 { get; set; }
            public string? nome { get; set; }
            public string? observacoes { get; set; }
            public bool? importadoIfap { get; set; }
            public bool? ativo { get; set; }
            public string? contacto { get; set; }


        }
        [Table("BusEntTipoContacto")]
        public class BusEntTipoContacto
        {
            [Key]

            public int codTipoContacto { get; set; }
            public string? codTipoContacto2 { get; set; }
            public string? tipoContacto { get; set; }

            public bool? ativo { get; set; }

        }

        [Table("TransactionDetails")]
        public class TransactionDetails
        {
            [Key]

            public long id { get; set; }
            public long all_trans_id { get; set; }
            public long tax_id { get; set; }
            public decimal unit_amount { get; set; }
            public int quantity { get; set; }

        }
        [Table("BillingAddress")]
        public class BillingAddress
        {
            [Key]

            public long id { get; set; }
            public int entity_id { get; set; }
            public string? address_line1 { get; set; }
            public string? address_line2 { get; set; }
         //   public string? city { get; set; }
        //    public string? state { get; set; }
        //    public string? country { get; set; }
            public DateTime? deleted_at { get; set; }
            public int? pin { get; set; }
            public int suggested { get; set; }
            public int? distrito_id { get; set; }
            public int? country_id { get; set; }
            public int? fregusia_id { get; set; }
            [NotMapped]
            public string? DESCON { get; set; }
            [NotMapped]
            public string? DESDIS { get; set; }

            [NotMapped]
            public string? DESFRG { get; set; }
            [NotMapped]
            public List<BusEntidadeContacto> busEntidadeContacto { get; set; }
    }

        [Table("DebitDetails")]
        public class DebitDetails
        {
            [Key]
            public long id { get; set; }
            public long debit_id { get; set; }
            public int quantity { get; set; }
            public decimal unit_amount { get; set; }


        }
       
        [Table("Cashier")]
        public class Cashier
        {
            public long id { get; set; }
            public string place { get; set; }

        }
        [Table("EntityAccounts")]
        public class EntityAccounts
        {
            public int id { get; set; }
            public int entity_id { get; set; }
            public string account { get; set; }
            public long account_num { get; set; }
            public DateTime? validity_date { get; set; }
            public DateTime? Reg_date { get; set; }
            public string other_details { get; set; }

        }
        [Table("IssueDocumentDetails")]
        public class IssueDocumentDetails
        {
            [Key]
            public long id { get; set; }
            public long ref_id { get; set; }
            public string? ref_table { get; set; }
            public string? document_type { get; set; }
            public string? document_url { get; set; }
            public string? invoice_url { get; set; }

            public decimal? document_size { get; set; }
            public int? status { get; set; }
            public int? cancel_invoice { get; set; }
            public DateTime? issue_date_time { get; set; }
            public DateTime? deadline_date { get; set; }
            public string? flag { get; set; }
            public string? extra_text { get; set; }
            public string? no { get; set; }
            public int? pag_limit { get; set; }
            public int entity_id { get; set; }
            public int? remove_tax { get; set; }
            public int? is_paid { get; set; }
            public int? total_item { get; set; }
            public int is_doc_generated { get; set; }
            public long? for_invoice_id { get; set; }

            public string? local { get; set; }

            public string? iva { get; set; }
            public double? amount_paid { get; set; }
            public double? total_amount { get; set; }
            public double? amount_to_be_used { get; set; }

            public double? service_tax { get; set; }
            public double? valor_cativo { get; set; }
            [NotMapped]
            public BusEntidade busEntidade { get; set; }

            [NotMapped]
            public List<AllTransactions> allTransaction { get; set; }


        }
        [Table("OpeningClosedAmount")]
        public class OpeningClosedAmount
        {
            [Key]
            public long id { get; set; }
            public int? entity_id { get; set; }
            public decimal? opening_amount { get; set; }
            public decimal? closed_amount { get; set; }
            public decimal? total_transaction { get; set; }
            public DateTime date { get; set; }
            public DateTime created_at { get; set; }
            public DateTime? updated_at { get; set; }
            public int? quantity { get; set; }
            public decimal? unit_amnt { get; set; }
            public string? caixa { get; set; }
            public decimal? cheques { get; set; }
            public decimal? numerario { get; set; }
            public long cashier_id { get; set; }
            public int closed { get; set; }


        }
        [Table("TaxType")]
        public class TaxType
        {
            public int id { get; set; }
            public decimal? tax_percent { get; set; }
            public string? tax_type { get; set; }

        }
        [Table("TransactionMethod")]
        public class TransactionMethod
        {
            public int id { get; set; }
            public string type { get; set; }

        }
        public class AllTransactions2
        {
            [Key]
            public long id { get; set; }
            public long parent_id { get; set; }
            public long billing_id { get; set; }
            public long? vat_no { get; set; }

            public int entity_id { get; set; }
            public long entityacc_Id { get; set; }
            public long? cashier_Id { get; set; }

            public long trans_no { get; set; }

            public string trans_type { get; set; }
            public string trans_msg { get; set; }

            public int trans_method_id { get; set; }
            public double? total_cr { get; set; }
            public double? total_dr { get; set; }
            public DateTime? trans_date { get; set; }
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

            public string despagerecord { get; set; }
            public decimal accumulated_previous { get; set; }
            public decimal bead_no { get; set; }
            [NotMapped]
            public List<IssueDocumentDetails> issueDocumentDetail { get; set; }

            [NotMapped]
            public List<BusEntidade> busEntidadeDetail { get; set; }
        }

        public class AllTransactions1
        {
            [Key]
            public long id { get; set; }
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
          
            public string despagerecord { get; set; }
            public decimal accumulated_previous { get; set; }
            public decimal bead_no { get; set; }
            [NotMapped]
            public List<IssueDocumentDetails> issueDocumentDetail { get; set; }
            [NotMapped]
            public BusEntidade busEntidadeDetail { get; set; }
            [NotMapped]
            public List<TransactionDetails> TransactionDetails { get; set; }

            [NotMapped]
            public string username { get; set; }
        }
        [Table("Currency")]
        public class Currency
        {
            [Key]
            public long id { get; set; }
            public string? currency { get; set; }
            public string? base_currency { get; set; }
            public double? conversion_rate { get; set; }

        }
        [Table("AllTransactions")]
        public class AllTransactions 
        {
            [Key]

            public long id { get; set; }
            public long parent_id { get; set; }
            public long currency_id { get; set; }
            public long? invoice_id { get; set; }
            public string? local { get; set; }


            public long? billing_id { get; set; }
            public long? vat_no { get; set; }

            public int entity_id { get; set; }
            public long entityacc_Id { get; set; }
            public long cashier_Id { get; set; }

            public long? trans_no { get; set; }

            public string trans_type { get; set; }
            public string trans_msg { get; set; }

            public int trans_method_id { get; set; }
            public double? total_cr { get; set; }
            public double? total_dr { get; set; }
            public DateTime trans_date { get; set; }
            public string? base_currency { get; set; }
            public string? trans_currency { get; set; }
            public DateTime Created_at { get; set; }
            public DateTime? deleted_at { get; set; }

            public string Comment { get; set; }
            public decimal captive_balance { get; set; }
            public decimal? vat_rate { get; set; }

            public double current_balance { get; set; }
            [NotMapped]
            public List<TransactionDetails> transactionDetail { get; set; }
            [NotMapped]
            public List<IssueDocumentDetails> issueDocumentDetail { get; set; }
            [NotMapped]
            public BillingAddress billingAddressDetail { get; set; }
            [NotMapped]
            public List<DebitDetails> debitDetai { get; set; }
            [NotMapped]

            public BusEntidade busEntidadeDetail { get; set; }

            public string bank { get; set; }

            public string? useful_cashier { get; set; }
            [NotMapped]

            public string cashier_place { get; set; }
            [NotMapped]

            public string nome { get; set; }
            public string despagerecord { get; set; }

            public decimal accumulated_previous { get; set; }

            public decimal bead_no { get; set; }

            [NotMapped]
            public string username { get; set; }

        }
      
    }
}
