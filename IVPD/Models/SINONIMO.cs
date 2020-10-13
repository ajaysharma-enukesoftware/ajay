using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public class SINONIMO
    {
        public int IDSINONIMO { get; set; }
        public int CODCASTA { get; set; }

        [Required]
        [MaxLength(50)]
        public string DESCASTA { get; set; }

        [Required]
        public decimal DTACT { get; set; }

        [Required]
        public decimal HRACT { get; set; }

        [Required]
        [MaxLength(25)]
        public string USR { get; set; }

        [Required]
        [MaxLength(25)]
        public string WKS { get; set; }
    }
}
