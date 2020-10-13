using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class PendingFiles
    {
        public string n_ficheiro { get; set; }
        public string estado { get; set; }
        public string entidade { get; set; }
        public string origem { get; set; }
        public string ficheiro { get; set; }
        public string n_movimento { get; set; }
        public string valor_total { get; set; }
        public string valor_retido { get; set; }
    }
}
