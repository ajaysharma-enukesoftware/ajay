using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class PaymentMade
    {
        public long N_Ficheiro { get; set; }
        public string Ent_que_paga { get; set; }
        public string Topi_pagamento { get; set; }
        public string Ent_recebe { get; set; }
        public string Nif_destino { get; set; }
        public string Valor_a_pagar { get; set; }
        public string Data_Entrada { get; set; }
        public string Data_transfer { get; set; }
        public string N_Carta { get; set; }
        public string Tipo_Prd { get; set; }
        public string Base { get; set; }
        public string Qnt { get; set; }
        public string Preco_pipa { get; set; }
        public string Valor_ja_Prago { get; set; }
        public string Outros_pagamentos { get; set; }
        public string Observacoes { get; set; }
        public string Menor_que { get; set; }
        public string Menor_ou_igual_a { get; set; }
        public string Igual { get; set; }
        public string Entre { get; set; }
        public string Maior_que { get; set; }
        public string Maior_ou_igual_a { get; set; }
        public string Date_picker_1 { get; set; }
        public string Date_picker_2 { get; set; }
        public string Transferencia { get; set; }
        public string Entrada { get; set; }
    }
}
