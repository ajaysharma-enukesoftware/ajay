using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class PaymentFileDetail
    {
        public string n_vit { get; set; }
        public string nome_ficheiro { get; set; }
        public string Entr { get; set; }
        public string Tipo_Prd { get; set; }
        public string valor_a_pagar { get; set; }
        public string quant { get; set; }
        public string v_pipa { get; set; }
        public string v_pago { get; set; }
        public string outros_pag { get; set; }
        public string data_pag { get; set; }
        public string estado { get; set; }

    }
}
