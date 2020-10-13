using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Models;
using IVPD.Helpers;
using Microsoft.EntityFrameworkCore;
using Audit.Core;
using System;
using Newtonsoft.Json;

namespace IVPD.Services
{

    public interface IAuditLogService
    {
        public List<AuditLogList> GetAll();
        public List<AuditLogList> GetAllParcel();
        public bool Create(AuditLog schedule);
        public bool CreateComment(string comment, long id);
        public bool CheckPermission(long userid, string modulename, string actionname);
    }

    public class AuditLogService : IAuditLogService
    {
        private IVPDContext _context;

        public AuditLogService(IVPDContext context)
        {
            _context = context;
        }
        public List<AuditLogList> GetAll()
        {
            List<AuditLog> data = _context.AuditLog.ToList();
            List<AuditLogList> alldata = new List<AuditLogList>();
            foreach (AuditLog item in data)
            {
                AuditLogList a = new AuditLogList();
                a = JsonConvert.DeserializeObject<AuditLogList>(JsonConvert.SerializeObject(item));
                a.Username = _context.Users.Where(w => w.Id == a.UserID).Select(s => s.FullName).FirstOrDefault();
                alldata.Add(a);
            }
            return alldata;
        }

        public List<AuditLogList> GetAllParcel()
        {
            List<AuditLog> data = _context.AuditLog.Where(w => w.Type.ToLower() == "parcel").ToList();
            List<AuditLogList> alldata = new List<AuditLogList>();
            foreach (AuditLog item in data)
            {
                AuditLogList a = new AuditLogList();
                a = JsonConvert.DeserializeObject<AuditLogList>(JsonConvert.SerializeObject(item));
                a.Username = _context.Users.Where(w => w.Id == a.UserID).Select(s => s.FullName).FirstOrDefault();
                alldata.Add(a);
            }
            return alldata;
        }
        public bool Create(AuditLog al)
        {
            try
            {
                al.Date = DateTime.UtcNow;
                al.Time = Convert.ToString(DateTime.UtcNow.TimeOfDay);
                _context.AuditLog.Add(al);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckPermission(long userid, string modulename, string actionname)
        {
            try
            {
                List<long> groupids = _context.UserGroups.Where(w => w.UserId == userid).Select(s => s.GroupId).ToList();
                long pid = _context.Permissions.Where(w => w.Name.ToLower() == modulename.ToLower()).FirstOrDefault().Id;
                List<bool> allbool = new List<bool>();
                switch (actionname.ToLower())
                {
                    case "isread":
                        foreach (long gid in groupids)
                        {
                            allbool.Add(_context.GroupPermissions.Where(w => w.Groupid == gid).Where(w => w.Permissionid == pid).Select(s => s.IsRead).FirstOrDefault());
                        }
                        break;
                    case "iswrite":
                        foreach (long gid in groupids)
                        {
                            allbool.Add(_context.GroupPermissions.Where(w => w.Groupid == gid).Where(w => w.Permissionid == pid).Select(s => s.IsWrite).FirstOrDefault());
                        }
                        break;
                    case "isupdate":
                        foreach (long gid in groupids)
                        {
                            allbool.Add(_context.GroupPermissions.Where(w => w.Groupid == gid).Where(w => w.Permissionid == pid).Select(s => s.IsUpdate).FirstOrDefault());
                        }
                        break;
                    case "isdelete":
                        foreach (long gid in groupids)
                        {
                            allbool.Add(_context.GroupPermissions.Where(w => w.Groupid == gid).Where(w => w.Permissionid == pid).Select(s => s.IsDelete).FirstOrDefault());
                        }
                        break;
                }
                return allbool.Any(x => x == true);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CreateComment(string comment, long id)
        {
            try
            {
                var auditlog = _context.AuditLog.FirstOrDefault(u => u.ID == id);
                auditlog.Comment = comment;
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
