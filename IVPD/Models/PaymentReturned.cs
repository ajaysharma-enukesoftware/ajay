using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class PaymentReturned
    {
        public long n_transferencia { get; set; }
        public string Data_pagamento { get; set; }
        public string n_viticultor { get; set; }
        public string nome { get; set; }
        public string nif { get; set; }
        public string tipo_production { get; set; }
        public string Base { get; set; }
        public string Valor { get; set; }
        public string N_Carta { get; set; }
        public string Tipo_Prd { get; set; }
        public string Qnt { get; set; }
        public string Preco_pipa { get; set; }
        public string Valor_ja_Prago { get; set; }
        public string Outros_pagamentos { get; set; }
        public string Observacoes { get; set; }
    }
}
