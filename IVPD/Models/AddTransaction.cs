using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class AddTransaction
    {
        public int entity_id { get; set; }
        public long trans_no { get; set; }
        public string trans_type { get; set; }
        public string trans_msg { get; set; }
        public double total_cr { get; set; }
        public string trans_date { get; set; }
        public string trans_currency { get; set; }
        public string comment { get; set; }
        public int trans_method_id { get; set; }
        public string bank { get; set; }
        public decimal bead_no { get; set; }

    }
    public class InvoiceLinkRequest
    {
        public long[] invoice_id { get; set; }
    }
}
