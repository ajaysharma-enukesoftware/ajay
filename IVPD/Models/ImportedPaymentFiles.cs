using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class ImportedPaymentFiles
    {
        public string n_ficheiro { get; set; }
        public string entidade { get; set; }
        public string ficheiro { get; set; }
        public string n_rec { get; set; }
        public string valor_total { get; set; }
        public string estado { get; set; }
    }
}
