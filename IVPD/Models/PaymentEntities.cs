using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class PaymentEntities
    {
        public string n_entidade { get; set; }
        public string entidade { get; set; }
        public string Nif { get; set; }
        public string Produto { get; set; }
        public string Base { get; set; }
        public string Quantidade_total { get; set; }
        public string valor_total { get; set; }
    }
}
