using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
   // [Table("V_SALDOSMG")]

    public class MGBalance
    {    
        public decimal PMGENT { get; set; }
        public string ENTNOM { get; set; }
        
        public int PMGPMB { get; set; }
        public int PMGPMT { get; set; }
        public int PMGPMR { get; set; }
        public int PMGCMB { get; set; }
        public int PMGCMT { get; set; }
        public int PMGCMR { get; set; }
        public int PMGADB { get; set; }
        public int PMGADT { get; set; }

        internal static object DefaultIfEmpty()
        {
            throw new NotImplementedException();
        }

        public int PMGADR { get; set; }
        public int PMGTCA { get; set; }
        public decimal TVINHO { get; set; }
        public decimal PMGSAL { get; set; }

        public int PMGTVD { get; set; }

        //     public int total_vinho { get; set; }
        //      public int vendas_em_base_V { get; set; }
        //      public int saldo { get; set; }

    }
 
    [Table("ENTPMG", Schema = "dbo")]
    public class EMTPMG
    { [Key]
        public decimal PMGENT { get; set; }

        public int PMGPMB { get; set; }
        public int PMGPMT { get; set; }
        public int PMGPMR { get; set; }
        public int PMGCMB { get; set; }
        public int PMGCMT { get; set; }
        public int PMGCMR { get; set; }
        public int PMGADB { get; set; }
        public int PMGADT { get; set; }
        public int PMGADR { get; set; }
        public int PMGTCA { get; set; }
        public int PMGTVD { get; set; }

    }
    [Table("ENTIDADES")]
    public class ENTIDADES
    {
        public string ENTNOM { get; set; }

        public decimal ENTNUM { get; set; }
      
    }

}
