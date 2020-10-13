using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class RegisterPrinting
    {
        [Key]
        public string n_folha { get; set; }
        public string n_de_carta { get; set; }
        public string motivo { get; set; }

    }
}
