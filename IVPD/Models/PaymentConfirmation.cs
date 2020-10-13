using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class PaymentConfirmation
    {
        public string n_ficheiro { get; set; }
        public string n_entidade { get; set; }
        public string entidade_pagadora { get; set; }
        public string entidade_beneficiaria { get; set; }
        public string carta { get; set; }
        public string valor { get; set; }
        public string data_pagamento { get; set; }

    }
}
