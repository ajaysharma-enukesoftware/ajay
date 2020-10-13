using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IVPD.Models
{
    public partial class Estatuto
    {
        public string Description { get; set; }

        [Key]
        public long CodEstatuto { get; set; }
        public bool? ModuleParcel { get; set; }
        public bool? ModuleHarvard { get; set; }
        public bool? ModuleFiscalization { get; set; }
        public bool? ModuloConvocatoria { get; set; }
        public bool? CurrentAccount { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

}
