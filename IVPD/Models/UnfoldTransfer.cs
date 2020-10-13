using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class UnfoldTransfer
    {
        public string n_entidade { get; set; }
        public string nome { get; set; }
        public string valor { get; set; }
        public string observacoes { get; set; }
    }
}
