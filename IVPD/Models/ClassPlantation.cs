using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    [Table("ClassePlant")]

    public class ClassPlantation
    {
        [MaxLength(50)]

        public string DesclassePlant { get; set; }
        [MaxLength(50)]

        public string Desprint { get; set; }
        [Required]
        public int Anoini { get; set; }
        [Required]

        public int Anofim { get; set; }

        [Required]

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDClassePlant { get; set; }
    }
  
}
