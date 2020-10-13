using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class BusEntDistrito
    {
        [Required]
        [MaxLength(19)]
        public string Desdis { get; set; }
        [Required]
        [MaxLength(1)]

        public string Status { get; set; }

        public string Cod { get; set; }
        [Required]

        [Key]
        public int Coddis { get; set; }
    }
   
}
