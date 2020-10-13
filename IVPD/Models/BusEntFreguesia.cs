using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IVPD.Models
{

    public class BusEntFreguesia
    {
        [Required]

        public string DICOFRE { get; set; }
        [Required]
        [MaxLength(1)]

        public string STATUS { get; set; }
        [Required]
        [MaxLength(53)]

        public string DESFRG  { get; set; }
        [Required]

        public int Idsubreg { get; set; }
        [Required]



        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ForeignKey("BusEntFreguesia")]

        public int CODDIS { get; set; }
        [ForeignKey("BusEntFreguesia")]
        [Required]

        public int CODCON { get; set; }
        [Required]

        public int CODFRG { get; set; }
    }

}
