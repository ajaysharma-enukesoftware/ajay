using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class Casta
    {
        [Required]
        [Key]
        public int Codcasta { get; set; }
        [Required]

        public byte Idcor { get; set; }
        [Required]

        public string Descasta { get; set; }
        public string Doporto { get; set; }
        public string Dodouro { get; set; }
        public string IG { get; set; }
        public string Mesa { get; set; }
        public string Veqprd { get; set; }
        public decimal? Dtact { get; set; }
        public decimal? Hract { get; set; }
        public string Usr { get; set; }
        public string Wks { get; set; }

        [Required]

        public int Idtpclassecasta { get; set; }
        [Required]

        public decimal? st { get; set; }
        [Required]

        public decimal? pct { get; set; }


    }
}
