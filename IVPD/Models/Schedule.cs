using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public partial class Schedules
    {
        public Int64 ID { get; set; }
        public string Entity { get; set; }
        public string Details { get; set; }
        public Int64? FromUserid { get; set; }
        public Int64? ToUserId { get; set; }
        public Int64? Profileid { get; set; }
        public string Title { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
        public string Observation { get; set; }
        public string PurposeTitle { get; set; }
        public string SourceModel { get; set; }
        public string Status { get; set; }
        public bool? Attendance { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsAllDay { get; set; }
        public string RecurrenceRule { get; set; }
    }
}
