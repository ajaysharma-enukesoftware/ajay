using System;
using System.ComponentModel.DataAnnotations;

namespace IVPD.Models
{
    public class BusEntidadeEstatuto
    {
        [Key]
        public int codEntidadeEstatuto { get; set; }
        public int? codEntidade { get; set; }
        public int? codEstatuto { get; set; }
    }

    public class BusEntidadeEstatutoList
    {
        public int codEntidade { get; set; }
        public string codEntidadeName { get; set; }
        public int[] codEstatuto { get; set; }
        public string codEstatutoName { get; set; }
    }
}
