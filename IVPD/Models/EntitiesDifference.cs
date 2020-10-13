using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class EntitiesDifference
    {
        public string n_entidade { get; set; }
        public string entidade { get; set; }
        public string transferenciaCP { get; set; }
        public string FicheirosRV { get; set; }
        public string Diferenca { get; set; }
    }
}
