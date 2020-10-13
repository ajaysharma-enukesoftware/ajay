using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class Receipts
    {
        public string ano { get; set; }
        public string n_ficha { get; set; }
        public string entidade_pag { get; set; }
        public string nif_pag { get; set; }
        public string tipo_pag { get; set; }
        public string data_transf { get; set; }
        public string destinatario { get; set; }
        public string nif_dest { get; set; }
        public string ent_rec { get; set; }
        public string nif_rec { get; set; }
        public string Tipo_Prd { get; set; }
        public string Base { get; set; }
        public string n_carta { get; set; }
        public string v_retido { get; set; }
        public string valor_a_pagar { get; set; }
        public string Qtd { get; set; }
        public string Preco_Pipa { get; set; }
        public string valor_ja_pago { get; set; }
        public string Outros_pag { get; set; }
    }
}
