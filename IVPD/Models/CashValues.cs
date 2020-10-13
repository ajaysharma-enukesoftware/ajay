using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static IVPD.Models.RevenueModels;

namespace IVPD.Models
{
    public class CashValues
    {
            public long id { get; set; }
            public long all_trans_id { get; set; }
            public long tax_id { get; set; }
            public int quantity { get; set; }
            public decimal unit_amount { get; set; }
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
            public DateTime Created_At { get; set; }

            public string Comment { get; set; }

            public double current_balance { get; set; }

        [NotMapped]
        public List<TransactionDetails> transactionDetail { get; set; }
        [NotMapped]
        public List<AllTransactions> allTransaction { get; set; }
    }
}
