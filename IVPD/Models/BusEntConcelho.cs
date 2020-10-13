using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{

    public class BusEntConcelho
    {
        [Required]
        [MaxLength(52)]

        public string Descon { get; set; }
        [Required]
        [MaxLength(1)]

        public string Status { get; set; }
        public string Cod { get; set; }
        [ForeignKey("BusEntConcelho")]
        [Required]

        public int Coddis { get; set; }
        [Required]

        public int Codcon { get; set; }
    }

}
