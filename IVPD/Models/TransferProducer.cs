using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class TransferProducer
    {
        public long n { get; set; }
        public string Mov_Orig { get; set; }
        public string n_ent { get; set; }
        public string entidade { get; set; }
        public string data { get; set; }
        public string valor { get; set; }
        public string tipo { get; set; }
        public string observacoes { get; set; }
    }
}
