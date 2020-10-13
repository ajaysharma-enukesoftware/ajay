using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class ConsultCurrentAccount
    {
        public decimal ANODOC { get; set; }
        public decimal NUMDOC { get; set; }
        public decimal DATDOC { get; set; }

        public decimal TIPDOC { get; set; }
        public decimal TOTDOC { get; set; }
        public decimal FMCDOC { get; set; }

        public decimal SVADOC { get; set; }
        public decimal IVADOC { get; set; }
        public string DOCTIP { get; set; }
        public string DESTIP { get; set; }
        public decimal SINTIP { get; set; }
        public decimal HORDOC { get; set; }
        public string UTIDOC { get; set; }
        public decimal INSDOC { get; set; }

    }
    [Table("LIBTESOUR_TSSALDF")]

    public class LIBTESOUR_TSSALDF
    {
        public decimal SALSAL { get; set; }
        public decimal INISAL { get; set; }
        public string VITSAL { get; set; }
        public decimal DATSAL { get; set; }
        public string UTISAL { get; set; }
        public decimal ARQSAL { get; set; }
        public decimal CATSAL { get; set; }
        public string MOTCAT { get; set; }
        public decimal MINSAL { get; set; }
        public decimal LV1SAL { get; set; }
        public string LV2SAL { get; set; }


    }
    [Table("LIBTESOUR_THDOCUF")]

    public class LIBTESOUR_THDOCUF
    {
        public decimal ANODOC { get; set; }
        public decimal TIPDOC { get; set; }
        public decimal NUMDOC { get; set; }
        public decimal DATDOC { get; set; }
        public decimal ENTDOC { get; set; }
        public decimal ENSDOC { get; set; }
        public decimal TOTDOC { get; set; }
        public decimal QTDDOC { get; set; }
        public decimal PRTDOC { get; set; }
        public decimal EXPDOC { get; set; }
        public decimal INSDOC { get; set; }

        public string UTIDOC { get; set; }
        public decimal HORDOC { get; set; }
        public decimal ESTDOC { get; set; }
        public string LOCDOC { get; set; }
        public string NOMDOC { get; set; }
        public string VITDOC { get; set; }
        public decimal IVADOC { get; set; }
        public string VT2DOC { get; set; }
        public decimal TPADOC { get; set; }
        public string DPADOC { get; set; }
        public string TPFDOC { get; set; }
        public string EFEDOC { get; set; }
        public decimal DFEDOC { get; set; }
        public decimal AN2DOC { get; set; }
        public decimal TP2DOC { get; set; }
        public decimal NU2DOC { get; set; }
        public decimal CTBDOC { get; set; }
        public string EATNUM { get; set; }
        public string SAFSTS { get; set; }
        public string NIFDOC { get; set; }
        public string MORDOC { get; set; }
        public decimal SVADOC { get; set; }
        public string UANDOC { get; set; }
        public decimal DANDOC { get; set; }
        public decimal HANDOC { get; set; }
        public string HASHOC { get; set; }
        public string HCTRLC { get; set; }
        public decimal FMCDOC { get; set; }


    }
    [Table("LIBTESOUR_TSTIPDF")]

    public class LIBTESOUR_TSTIPDF
    {
        public decimal ANOTIP { get; set; }
        public decimal CODTIP { get; set; }
        public decimal SERTIP { get; set; }
        public string DOCTIP { get; set; }
        public string DESTIP { get; set; }
        public string DESEXT { get; set; }
        public decimal SINTIP { get; set; }
        public decimal SINCTB { get; set; }
        public string CNTTIP { get; set; }
        public string DCCTIP { get; set; }
        public string ANUTIP { get; set; }
        public string ATCOD { get; set; }
        public decimal ATNUM { get; set; }



    }
}
