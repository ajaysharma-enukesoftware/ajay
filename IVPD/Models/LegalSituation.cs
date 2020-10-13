using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    [Table("SITLEGAL", Schema = "Parcela")]

    public class LegalSituation
    {
      
        public string Dessitleg { get; set; }
        public bool Licencaivv { get; set; }

        public decimal Dtact { get; set; }
        public decimal Hract { get; set; }
        public string Usr { get; set; }
        public string Wks { get; set; }
     
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public byte Idsitleg { get; set; }

    }

   

}
