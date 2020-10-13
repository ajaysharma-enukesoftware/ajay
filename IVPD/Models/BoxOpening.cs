using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Models
{

    public class BoxOpening
    {
        //  string sQuery = "select  AllTransactions.id,entity_id,entityacc_Id,trans_no,Trans_type,trans_msg,trans_method,total_cr,total_dr,current_balance,trans_date,Base_Currency,trans_currency,Created_at,Comment,TransactionDetails.all_trans_id,tax_id,unit_amount,quantity from AllTransactions  INNER JOIN TransactionDetails ON AllTransactions.id = TransactionDetails.all_trans_id  FOR JSON AUTO";

       /*   public long parent_id { get; set; }


          public long id { get; set; }
        public long cashier_Id { get; set; }

        public int entity_id { get; set; }

          public long entityacc_Id { get; set; }


          public long? trans_no { get; set; }

          public string trans_type { get; set; }

          public string trans_msg { get; set; }


          public string trans_method { get; set; }

          public double? total_cr { get; set; }

          public double? total_dr { get; set; }

          public DateTime trans_date { get; set; }

          public string base_currency { get; set; }

          public string trans_currency { get; set; }

          public DateTime Created_at { get; set; }
          public string Comment { get; set; }
          public double current_balance { get; set; }*/
        public string place { get; set; }

     //   [NotMapped]
      //  public List<TransactionDetails> transactionDetail { get; set; }
    //    [NotMapped]
     //   public List<AllTransactions> allTransaction { get; set; }

    }
    public class BoxOpeningList
    {
        public AllTransactions[] allTransactions { get; set; }
        public TransactionDetails[] transactionDetails { get; set; }

    }

}
