using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string AdminEmail { get; set; }
        public string SMTPEmail { get; set; }
        public string SMTPPWD { get; set; }
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string ClientID { get; set; }
        public string ResourceID { get; set; }
        public string TenantID { get; set; }
        public string ClientSecret { get; set; }
        public string CallbackPath { get; set; }
        public string Instance { get; set; }
    }
}
