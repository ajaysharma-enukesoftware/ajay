using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class RegisterMovements
    {
        public string n_viticultor { get; set; }
        public string Tipo_Prd { get; set; }
        public string Base { get; set; }
        public string Valor_a_pagar { get; set; }
        public string Valor_retido { get; set; }
        public string Quantidade { get; set; }
        public string valor_pipa { get; set; }
        public string valor_pago { get; set; }
        public string outros_pagamentos { get; set; }

    }
}
