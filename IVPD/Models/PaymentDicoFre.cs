using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class PaymentDicoFre
    {
        public string n_entidade { get; set; }
        public string C_Postal { get; set; }
        public string entidade { get; set; }
        public string Entidade_Recetora { get; set; }
        public string Nif_entidade { get; set; }
        public string nif_pagadora { get; set; }
        public string Valor_recebido { get; set; }
    }
}
