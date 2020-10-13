using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class PendingPayment
    {
        public string ano { get; set; }
        public string n_entidade { get; set; }
        public string Entidade_que_paga { get; set; }
        public string nib { get; set; }
        public string Entidade_que_recebe { get; set; }
        public string estado { get; set; }
        public string nif { get; set; }
        public string tipo_prd { get; set; }
        public string Base { get; set; }
        public string valor_a_pagar { get; set; }

    }
}
