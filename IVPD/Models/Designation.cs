using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public partial class Designation
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public partial class ForDesignationList
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
