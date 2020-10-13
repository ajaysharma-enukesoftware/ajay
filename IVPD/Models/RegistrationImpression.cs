using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    [Table("REGCARTAS")]

    public class REGCARTAS
    {
        [Key]
        public decimal RCBNUM { get; set; }
        public decimal RCBDES { get; set; }
        public string RCBNCA { get; set; }
        public decimal RCBDAT { get; set; }
        public decimal RCBHOR { get; set; }

        public string RCBUSR { get; set; }
        public string RCBWKS { get; set; }
        public string RCBSTS { get; set; }
        public decimal RCBDT2 { get; set; }
        public decimal RCBHR2 { get; set; }
        public string RCBUR2 { get; set; }
        public string RCBWS2 { get; set; }
        public string RCBMOT { get; set; }
    }
    [Table("FRBLIN")]

    public class FRBLIN
    {
        public string FBLSTS { get; set; }
        public string FBLNCA { get; set; }

    }
    public class RegistrationImpression
    {
        public decimal RCBNUM { get; set; }
        public decimal RCBDES { get; set; }
        public string RCBNCA { get; set; }
        public decimal RCBDAT { get; set; }

        public string RCBUSR { get; set; }
        public string RCBWKS { get; set; }
        public string RCBSTS { get; set; }
        public decimal RCBDT2 { get; set; }
        public decimal RCBHR2 { get; set; }
        public string RCBUR2 { get; set; }
        public string RCBWS2 { get; set; }
        public string RCBMOT { get; set; }
        public string ENTNOM { get; set; }
        public string FBLSTS { get; set; }

        

    }
}
