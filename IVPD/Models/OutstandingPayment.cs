using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class OutstandingPayment
    {
        public string n_ficheiro { get; set; }
        public string n_entidade { get; set; }
        public string Ent_que_paga { get; set; }
        public string Ent_que_recebe { get; set; }
        public string n_entidade2 { get; set; }
        public string nif { get; set; }
        public string nib { get; set; }
        public string Base { get; set; }
        public string Tipo_Prd { get; set; }
        public string Qnt { get; set; }
        public string Preco_pipa { get; set; }
        public string Valor_ja_Prago { get; set; }
        public string Outros_pagamentos { get; set; }

    }
}
