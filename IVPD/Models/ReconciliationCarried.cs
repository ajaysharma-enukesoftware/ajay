using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class ReconciliationCarried
    {

        public string data { get; set; }
        public string  Transf_p_CP{ get; set; }
        public string  C_Ficheiros{ get; set; }
        public string S_Ficheiro { get; set; }
        public string PagRealizados { get; set; }
        public string PagDevolvidos { get; set; }
        public string Retidos { get; set; }
        public string  Pendentes { get; set; }
        public string não_assinados{ get; set; }
        public string SaldoApurado{ get; set; }
        public string SaldoExtrato{ get; set; }
        public string observações { get; set; }
        public string utilizador { get; set; }

    }
}
