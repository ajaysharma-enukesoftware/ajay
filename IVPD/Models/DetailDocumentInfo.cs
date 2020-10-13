using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class DetailDocumentInfo
    {
        public decimal CTBDOL { get; set; }
        public decimal DTEDOL { get; set; }
        public string DESIVA { get; set; }

        public decimal LINDOL { get; set; }
        public decimal QTDDOL { get; set; }
        public string UNIDOL { get; set; }
        public decimal VALDOL { get; set; }
        public decimal IVADOL { get; set; }
        public decimal TXAIVA { get; set; }
        public string REFDOL { get; set; }
        public string DESDOL { get; set; }


    }
    [Table("LIBTESOUR_TSCIVAF")]

    public class LIBTESOUR_TSCIVAF
    {
        public decimal VALIVA { get; set; }
        public string DESIVA { get; set; }
        public string CTBIVA { get; set; }

    }
    [Table("LIBTESOUR_THDOCLF")]

    public class LIBTESOUR_THDOCLF
    {
        public decimal ANODOL { get; set; }
        public decimal TOPDOL { get; set; }
        public decimal NUMDOL { get; set; }
        public decimal LINDOL { get; set; }
        public decimal QTDDOL { get; set; }
        public string UNIDOL { get; set; }
        public decimal VALDOL { get; set; }
        public string REFDOL { get; set; }
        public string DESDOL { get; set; }
        public decimal CTBDOL { get; set; }
        public decimal DTEDOL { get; set; }
        public decimal CUSTON { get; set; }
        public decimal TAXPRO { get; set; }
        public decimal TAXCER { get; set; }
        public string CMSTPO { get; set; }
        public decimal VLTUNI { get; set; }
        public decimal VLTPRO { get; set; }
        public decimal VLTCER { get; set; }
        public decimal TAXCTR { get; set; }
        public decimal VLTCTR { get; set; }
        public decimal IVADOL { get; set; }
        public string ARTDOL { get; set; }
        public decimal TXAIVA  { get; set; }

    }
}
