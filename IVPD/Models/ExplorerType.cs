using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    [Table("TIPOEXPLOR", Schema = "Parcela")]

    public class ExplorerType
    {  [Key]
        public int IDTIPOEXPLOR { get; set; }
        public string DESTIPOEXPLOR { get; set; }
       
    }

}
