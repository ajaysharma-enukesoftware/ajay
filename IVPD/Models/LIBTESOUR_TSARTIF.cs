using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    [Table("LIBTESOUR_TSARTIF$")]
    public class LIBTESOUR_TSARTIF
    {
        [Key]
        public long id { get; set; }

        public string? CODART { get; set; }
            public string? DESART { get; set; }
            public double? VALART { get; set; }
            public double? CTBART { get; set; }
            public string? RECART { get; set; }
            public string? UNIART { get; set; }
            public string? NUMART { get; set; }
            public string? ESTART { get; set; }
            public string? NETART { get; set; }
            public string? TIPART { get; set; }
            public string? ORIART { get; set; }


        
    }
}
