using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class BoxClasp
    {

        public string DisplayName { get; set; }

        public long id { get; set; }
        public int entity_id { get; set; }
        public long entityacc_Id { get; set; }
        public long cashier_Id { get; set; }
        public long parent_id { get; set; }

        public long? trans_no { get; set; }

        public string trans_type { get; set; }
        public string trans_msg { get; set; }

        public int trans_method_id { get; set; }
        public double? total_cr { get; set; }
        public double? total_dr { get; set; }
        public DateTime trans_date { get; set; }
        public string base_currency { get; set; }
        public string trans_currency { get; set; }
        public DateTime Created_At { get; set; }

        public string Comment { get; set; }
        public string type { get; set; }

        public double current_balance { get; set; }
   /*     public long all_trans_id { get; set; }
        public long tax_id { get; set; }
        public int quantity { get; set; }
        public decimal unit_amount { get; set; }*/


    }
}
