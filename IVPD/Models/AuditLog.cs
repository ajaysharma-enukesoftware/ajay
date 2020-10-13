using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public class AuditLog
    {
        public long ID { get; set; }
        public string Type { get; set; }
        public string Activity { get; set; }
        public string Comment { get; set; }
        public DateTime? Date { get; set; }
        public string Time { get; set; }
        public long? UserID { get; set; }
        public string OldRecord { get; set; }
        public string NewRecord { get; set; }
    }

    public class AuditLogList : AuditLog
    { 
        public string Username { get; set; }
    }
}
