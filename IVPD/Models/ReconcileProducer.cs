using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IVPD.Models
{
    [Table("CONCILIACAO_CP_ANO")]

    public partial class RecocileProducer
    {
        public decimal CONCDAT { get; set; }
        public int CONCANO { get; set; }

        public decimal V_TRANSCP { get; set; }
        public decimal V_FICHRV { get; set; }
        public decimal V_SEMFICHRV { get; set; }
        public decimal V_PAGREALIZADOS { get; set; }
        public decimal V_PAGDEVOLVIDOS { get; set; }
        public decimal V_PAGRETIDOS { get; set; }
        public decimal V_PAGPENDENTES { get; set; }
        public decimal V_PAGNA { get; set; }
        public decimal SALDO_APURADO  { get; set; }
    }
}
