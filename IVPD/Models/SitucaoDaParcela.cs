using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    [Table("SITPARCELA", Schema = "Parcela")]

    public class SitucaoDaParcela
    {
        [Required]
        [MaxLength(75)]
        public string Dessitparc { get; set; }
        [Required]

        public decimal Dtact { get; set; }
        [Required]

        public decimal Hract { get; set; }
        [Required]
        [MaxLength(25)]

        public string Usr { get; set; }
        [Required]
        [MaxLength(25)]

        public string Wks { get; set; }
        [Required]


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public byte Idsitparc { get; set; }
    }
}
