using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class FilesCreateBank
    {
        public string n_ficheiro { get; set; }
        public string entidade { get; set; }
        public string n_rec { get; set; }
        public string valor_total { get; set; }
        public string estado { get; set; }
        public string dgt { get; set; }
        public string n_tei { get; set; }
        public string data_hora { get; set; }
        public string utilizador { get; set; }
    }
}
