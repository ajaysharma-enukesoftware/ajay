using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class PaymentNotSent
    {   
        public string Estado { get; set; }
        public string n_movimento { get; set; }
        public string montante { get; set; }
        public string data_ficheiro { get; set; }

    }
}
