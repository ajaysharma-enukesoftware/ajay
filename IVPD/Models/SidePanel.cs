using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class SidePanel
    {
        public string n_entidade { get; set; }
        public string nome { get; set; }
        public string data { get; set; }
        public string valor { get; set; }
        public string tipo { get; set; }
        public string observacoes { get; set; }
    }
}
