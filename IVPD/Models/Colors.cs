using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class colors
    {
        [Key]
        public byte Idcor { get; set; }
        public string Descor { get; set; }
       
    }

}
