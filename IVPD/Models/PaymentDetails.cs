using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class PaymentDetails
    {
        public string n_ficheiro { get; set; }
        public string ficheiro { get; set; }
        public string Entr { get; set; }
        public string Tipo_Prd { get; set; }
        public string valor_a_pagar { get; set; }
        public string Base { get; set; }
    }
}
