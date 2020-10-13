using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    [Table("APLIN", Schema = "AP")]

    public class ProductionAuthorization
    {

        public int NRAP { get; set; }
        public string ENTNUM { get; set; }

        public int NUMPARC { get; set; }
        [Required]
        public short VERSAO { get; set; }
        [Required]

        public int CODDIS { get; set; }
        [Required]

        public int CODCON { get; set; }
        [Required]

        public int CODFRG { get; set; }
        [Required]

        public string DESDIS { get; set; }
        [Required]


        public string DESCON { get; set; }
        [Required]

        public string DESFRG { get; set; }
        [Required]

        public string DESPARC { get; set; }
        [Required]

        public string CLASSE { get; set; }
        [Required]

        public int IDSITPARC { get; set; }
        [Required]

        public int IDSITLEG { get; set; }
        [Required]

        

        public string SITDCP { get; set; }
        [Required]

        public string CODLITIGIO { get; set; }
        [Required]

        public decimal AREAAPTA { get; set; }
        [Required]

        public decimal AREAAF { get; set; }
        [Required]

        public decimal AREANAPTA { get; set; }
        [Required]

        public decimal AREAILEGAL { get; set; }
        [Required]

        public decimal AREATOTAL { get; set; }
        [Required]

        public decimal PRODBRIG { get; set; }
        [Required]

        public decimal PCTMOSCATEL { get; set; }
        [Required]

        public decimal PCTBRANCA { get; set; }
        [Required]

        public decimal QTMG { get; set; }
        [Required]

        public decimal QTRE { get; set; }
        [Required]

        public DateTime DTINSERT { get; set; }
        [Required]

        public string USR { get; set; }
        [Required]

        public string WKS { get; set; }




    }
}
