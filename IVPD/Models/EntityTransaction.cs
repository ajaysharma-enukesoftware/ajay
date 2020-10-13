using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Models
{
  /*
   *BillingAddress b = _context.BillingAddress.AsNoTracking().Where(w => w.entity_id == u.entity_id).ToList().FirstOrDefault();
    BusEntidade bus = _context.BusEntidade.AsNoTracking().Where(w => w.nifap == u.entity_id.ToString()).ToList().FirstOrDefault();
    IssueDocumentDetails issue = _context.IssueDocumentDetails.AsNoTracking().Where(w => w.ref_id == u.id).ToList().FirstOrDefault();
    TransactionMethod tm = _context.TransactionMethod.AsNoTracking().Where(w => w.id == u.trans_method_id).ToList().FirstOrDefault();

List<TransactionMethod> tm2 = _context.TransactionMethod.AsNoTracking().ToList();
*/
public class EntityTransaction
   {
        //[NotMapped]
        public List<AllTransactions> allTransaction { get; set; }
        public List<DebitDetails> debitDetail { get; set; }

        //  [NotMapped]
        public BusEntidade busEntidade { get; set; }
      //  [NotMapped]
        public List<BillingAddress> billingAddress { get; set; }
      //  [NotMapped]
        public List<IssueDocumentDetails> issueDocumentDetails { get; set; }
      //  [NotMapped]
        public TransactionMethod transactionMethod { get; set; }
    
     //   [NotMapped]
        public List<TransactionMethod> transactionMethodlist { get; set; }

        public double currentEntityBalance { get; set; }
}
    public class EntityDetails
    {
        //[NotMapped]
     //   public List<AllTransactions> allTransaction { get; set; }
     //   public List<DebitDetails> debitDetail { get; set; }

        //  [NotMapped]
        public BusEntidade busEntidade { get; set; }
        //  [NotMapped]
        public List<BillingAddress> billingAddress { get; set; }
        // public List<AllTransactions> allTransactions { get; set; }

        //  [NotMapped]
        //  [NotMapped]

        //   [NotMapped]
        public decimal captive_balance { get; set; }
        public decimal accumulated_previous { get; set; }

        public double? currentEntityBalance { get; set; }
    }
    public class TransactionDelete
    {     
        public long transId { get; set; }
    }
    public class ProcessInvoiceDetails
    {
        public long invoiceId { get; set; }
    }
    public class EntityTransactionDetailRequest
    {
        public long transId { get; set; }
        public int entityId { get; set; }


    }
    public class EntityDetailsRequest
    {  
        public int entityId { get; set; }
    }
    public class EntityAccountDetailRequest
    {
        public Dictionary<string, object> Filters { get; set; }
        public string SortBy { get; set; }
        public bool? IsSortTypeDESC { get; set; }
        public bool? IsPagination { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int entityId { get; set; }


    }
    public class EntityAccountDetail
    {
        public string? nome { get; set; }
        public List<AllTransactions> allTransactions { get; set; }


    }
    public class ProcessInvoice
    {
        public AllTransactions allTransactions { get; set; }
        public IssueDocumentDetails issueDocumentDetails { get; set; }
        public string nome { get; set; }
        public List<BillingAddress> billingAddress { get; set; }


    }

}
