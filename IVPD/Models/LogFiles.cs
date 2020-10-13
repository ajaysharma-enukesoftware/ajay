using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class LogFiles
    {
        public long N_entidade { get; set; }
        public string entidade { get; set; }
        public string Valor_total { get; set; }
    }
}
