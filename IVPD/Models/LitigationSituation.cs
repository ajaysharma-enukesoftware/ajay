using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    [Table("SITLITIGIO", Schema = "Parcela")]
    public class LitigationSituation
    {
        public string Deslitigio { get; set; }

        public decimal Dtact { get; set; }
        public decimal Hract { get; set; }
        public string Usr { get; set; }
        public string Wks { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string CODLITIGIO { get; set; }
    }
  
}
