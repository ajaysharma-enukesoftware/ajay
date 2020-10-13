using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public class MatrixList
    {
        public long id { get; set; }
        public string SourceCode { get; set; }
        public string Plantationbuilding { get; set; }
        public string Starts { get; set; }
        public string Ends { get; set; }
        public string PlantingYear { get; set; }
        public string Types { get; set; }
        public string TypeofLegislation { get; set; }
        public string AvailableAreaDouro { get; set; }
        public string AvailableDouro { get; set; }
        public string AvailablePorto { get; set; }
        public string RightCode { get; set; }
        public string TotalArea { get; set; }
    }

}
