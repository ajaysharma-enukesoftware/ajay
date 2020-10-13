using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Models;
using IVPD.Helpers;
using Microsoft.EntityFrameworkCore;
using Audit.Core;
using System;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace IVPD.Services
{

    public interface IScheduleService
    {
        public List<Schedules> GetAll();
        public Schedules GetById(Int64 Id);
        public APIResponse ResendSchedule(long id, string AdminEmail, string SMTPEmail, string SMTPPwd, int SMPTPort, string SMTPMail);
        public APIResponse ApprovedSchedule(long id, string AdminEmail, string SMTPEmail, string SMTPPwd, int SMPTPort, string SMTPMail);

        public APIResponse Create(Schedules schedule, string AdminEmail, string SMTPEmail, string SMTPPwd, int SMPTPort, string SMTPMail);
        public APIResponse Delete(int id, string AdminEmail, string SMTPEmail, string SMTPPwd, int SMPTPort, string SMTPMail);
        public APIResponse Update(Schedules schedule, string AdminEmail, string SMTPEmail, string SMTPPwd, int SMPTPort, string SMTPMail);
    }

    public class ScheduleService : IScheduleService
    {
        private IVPDContext _context;

        public ScheduleService(IVPDContext context)
        {
            _context = context;
        }
        public List<Schedules> GetAll()
        {
            return _context.Schedules.Where(w => w.DeleteAt == null).Select(s => s).ToList();
        }

        public Schedules GetById(Int64 Id)
        {
            if (Id == 0)
            {
                throw new AppException("Id is Required");
            }

            return _context.Schedules.AsNoTracking().Where(x => x.ID == Id).Where(w => w.DeleteAt == null).Select(s => s).ToList().FirstOrDefault();
        }

        public APIResponse Delete(int id, string AdminEmail, string SMTPEmail, string SMTPPwd, int SMPTPort, string SMTPMail)
        {
            var schedule = _context.Schedules.Where(w => w.ID == id).ToList().FirstOrDefault();
            if (schedule != null)
            {

                string subject = schedule.Title;
                User u = _context.Users.AsNoTracking().Where(w => w.Id == schedule.ToUserId).Select(s => s).FirstOrDefault();
                schedule.DeleteAt = DateTime.UtcNow;
                _context.Schedules.Update(schedule);
                _context.SaveChanges();
                if (u != null)
                {
                    User fromUserId = _context.Users.AsNoTracking().Where(w => w.Id == schedule.FromUserid).Select(s => s).FirstOrDefault();

                    string Username = u.FullName;

                    string mailsentTO = u.Email;
                    string source = System.IO.File.ReadAllText("StaticFiles/DeleteSchedule.html");

                    string date = source.Replace("{date}", schedule.Date.Value.Date.ToShortDateString());
                    string time = date.Replace("{time}", Convert.ToDateTime(schedule.Time).ToShortTimeString());
                    string username = time.Replace("{username}", u.FullName);

                    string result = username.Replace("{details}", schedule.Details); string content, res;

                    if (fromUserId != null)
                    {
                        content = result.Replace("{fromusername}", fromUserId.FullName);
                    }
                    else
                    {
                        content = result.Replace("{fromusername}", "");

                    }

                    res = CommonFunctions.SendSchedule(content, mailsentTO, Username, subject, AdminEmail, SMTPEmail, SMTPPwd, SMPTPort, SMTPMail);
                    if (res == "MailSent")
                    {
                        return new APIResponse(true, "", "Schedule Cancelled Successfully and Mail Sent Successfully!");

                    }
                    else
                    {
                        return new APIResponse(true, res, "Schedule Cancelled Successfully and Mail Not Sent !");

                    }
                }
                else
                {
                    return new APIResponse(true, "", "Schedule Cancelled Successfully!");

                }

            }
            else
            {
                throw new AppException("no Schedule Exist with the given id");
            }
        }
        public APIResponse ResendSchedule(long id, string AdminEmail, string SMTPEmail, string SMTPPwd, int SMPTPort, string SMTPMail)
        {
            try
            {
                Schedules schedule = _context.Schedules.AsNoTracking().Where(w => w.ID == id).Select(s => s).FirstOrDefault();
                if (schedule != null)
                {
                    string subject = schedule.Title;
                    User u = _context.Users.Where(w => w.Id == schedule.ToUserId).Select(s => s).FirstOrDefault();
                    User fromUserId = _context.Users.AsNoTracking().Where(w => w.Id == schedule.FromUserid).Select(s => s).FirstOrDefault();

                    if (u != null)
                    {
                        string source = System.IO.File.ReadAllText("StaticFiles/CreateSchedule.html");

                        string Username = u.FullName;

                        string mailsentTO = u.Email;
                        string date = source.Replace("{date}", schedule.Date.Value.Date.ToShortDateString());
                        string time = date.Replace("{time}", Convert.ToDateTime(schedule.Time).ToShortTimeString());
                        string username = time.Replace("{username}", u.FullName);

                        string result = username.Replace("{details}", schedule.Details); string content, res;
                        if (fromUserId != null)
                        {
                            content = result.Replace("{fromusername}", fromUserId.FullName);
                        }
                        else
                        {
                            content = result.Replace("{fromusername}", "");

                        }


                        res = CommonFunctions.SendSchedule(content, mailsentTO, Username, subject, AdminEmail, SMTPEmail, SMTPPwd, SMPTPort, SMTPMail);
                        if (res == "MailSent")
                        {
                            return new APIResponse(true, "", "Schedule Resend Successfully and Mail Sent Successfully!");

                        }
                        else
                        {
                            return new APIResponse(true, res, "Schedule Resend Successfully and Mail Not Sent !");

                        }
                    }
                    else
                    {
                        return new APIResponse(true, "", "Schedule Resend Successfully!");

                    }

                }
                else
                {
                    return new APIResponse(false, "", "No Schedule Found !");

                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }


        public APIResponse ApprovedSchedule(long id, string AdminEmail, string SMTPEmail, string SMTPPwd, int SMPTPort, string SMTPMail)
        {
            try
            {
                Schedules schedule = _context.Schedules.AsNoTracking().Where(w => w.ID == id).Select(s => s).FirstOrDefault();
                if (schedule != null)
                {
                    string subject = schedule.Title;
                    User u = _context.Users.Where(w => w.Id == schedule.ToUserId).Select(s => s).FirstOrDefault();
                    User fromUserId = _context.Users.AsNoTracking().Where(w => w.Id == schedule.FromUserid).Select(s => s).FirstOrDefault();

                    if (u != null)
                    {

                        string source = System.IO.File.ReadAllText("StaticFiles/ApprovedSchedule.html");
                        string Username = u.FullName;
                        string mailsentTO = u.Email;
                        string date = source.Replace("{date}", schedule.Date.Value.Date.ToShortDateString());
                        string time = date.Replace("{time}", Convert.ToDateTime(schedule.Time).ToShortTimeString());
                        string username = time.Replace("{username}", u.FullName);

                        string result = username.Replace("{details}", schedule.Details);
                        string content, res;
                        if (fromUserId != null)
                        {
                            content = result.Replace("{fromusername}", fromUserId.FullName);
                        }
                        else
                        {
                            content = result.Replace("{fromusername}", "");

                        }


                        res = CommonFunctions.SendSchedule(content, mailsentTO, Username, subject, AdminEmail, SMTPEmail, SMTPPwd, SMPTPort, SMTPMail);
                        if (res == "MailSent")
                        {
                            return new APIResponse(true, "", "Schedule Approved Successfully and Mail Sent Successfully!");

                        }
                        else
                        {
                            return new APIResponse(true, res, "Schedule Approved Successfully and Mail Not Sent !");

                        }
                    }
                    else
                    {
                        return new APIResponse(true, "", "Schedule Approved Successfully!");

                    }

                }
                else
                {
                    return new APIResponse(false, "", "No Schedule Found !");

                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }
        public APIResponse Create(Schedules schedule, string AdminEmail, string SMTPEmail, string SMTPPwd, int SMPTPort, string SMTPMail)
        {
            try
            {
                if (string.IsNullOrEmpty(schedule.Title))
                {
                    throw new AppException("Schedule Title is required!");
                }
                else if (string.IsNullOrEmpty(schedule.Entity))
                {
                    throw new AppException("Schedule Entity is required!");
                }
                else if (string.IsNullOrEmpty(schedule.Observation))
                {
                    throw new AppException("Schedule Observation is required!");
                }
                else
                {
                    schedule.CreatedAt = DateTime.UtcNow;
                    _context.Schedules.Add(schedule);
                    _context.SaveChanges();
                    string subject = schedule.Title;
                    User u = _context.Users.AsNoTracking().Where(w => w.Id == schedule.ToUserId).Select(s => s).FirstOrDefault();
                    User fromUserId = _context.Users.AsNoTracking().Where(w => w.Id == schedule.FromUserid).Select(s => s).FirstOrDefault();
                    string res;
                    if (u != null)
                    {

                        string source = System.IO.File.ReadAllText("StaticFiles/CreateSchedule.html");

                        string date = source.Replace("{date}", schedule.Date.Value.Date.ToShortDateString());
                        string time = date.Replace("{time}", Convert.ToDateTime(schedule.Time).ToShortTimeString());
                        string username = time.Replace("{username}", u.FullName);
                        string result = username.Replace("{details}", schedule.Details);
                        string Username = u.FullName;

                        string mailsentTO = u.Email;
                        string content;
                        if (fromUserId != null)
                        {
                            content = result.Replace("{fromusername}", fromUserId.FullName);
                        }
                        else
                        {
                            content = result.Replace("{fromusername}", "");
                        }

                        res = CommonFunctions.SendSchedule(content, mailsentTO, Username, subject, AdminEmail, SMTPEmail, SMTPPwd, SMPTPort, SMTPMail);
                        if (res == "MailSent")
                        {
                            return new APIResponse(true, "", "Schedule Created Successfully and Mail Sent Successfully!");

                        }
                        else
                        {
                            return new APIResponse(true, res, "Schedule Created Successfully and Mail Not Sent !");

                        }
                    }
                    else
                    {
                        return new APIResponse(true, "", "Schedule Created Successfully!");
                    }

                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }
        }


        public APIResponse Update(Schedules schedule, string AdminEmail, string SMTPEmail, string SMTPPwd, int SMPTPort, string SMTPMail)
        {
            try
            {
                if (string.IsNullOrEmpty(schedule.Title))
                {
                    throw new AppException("Schedule Title is required!");
                }
                else if (string.IsNullOrEmpty(schedule.Entity))
                {
                    throw new AppException("Schedule Entity is required!");
                }
                else if (string.IsNullOrEmpty(schedule.Observation))
                {
                    throw new AppException("Schedule Observation is required!");
                }
                else
                {
                    schedule.UpdatedAt = DateTime.UtcNow;
                    _context.Schedules.Update(schedule);
                    _context.SaveChanges();
                    string subject = schedule.Title;
                    User u = _context.Users.AsNoTracking().Where(w => w.Id == schedule.ToUserId).Select(s => s).FirstOrDefault();
                    User fromUserId = _context.Users.AsNoTracking().Where(w => w.Id == schedule.FromUserid).Select(s => s).FirstOrDefault();

                    if (u != null)
                    {
                        /*   string mailsentTO = u.Email;
                           string Username = u.FullName;
                           string content = @"Dear " + u.FullName + " , scheduled for " + schedule.Details + " at " + schedule.Date.Value.Date.ToShortDateString() + "  " +
                                              Convert.ToDateTime(schedule.Time).ToShortTimeString() + "<br/><br/> Thanks";
                           CommonFunctions.SendSchedule(content, mailsentTO, Username, subject, AdminEmail, SMTPEmail, SMTPPwd, SMPTPort, SMTPMail);
                           */
                        string source = System.IO.File.ReadAllText("StaticFiles/UpdateSchedule.html");

                        string date = source.Replace("{date}", schedule.Date.Value.Date.ToShortDateString());
                        string time = date.Replace("{time}", Convert.ToDateTime(schedule.Time).ToShortTimeString());
                        string username = time.Replace("{username}", u.FullName);

                        string result = username.Replace("{details}", schedule.Details);
                        string res, content;
                        string Username = u.FullName;

                        string mailsentTO = u.Email;
                        if (fromUserId != null)
                        {
                            content = result.Replace("{fromusername}", fromUserId.FullName);
                        }
                        else
                        {
                            content = result.Replace("{fromusername}", "");

                        }

                        res = CommonFunctions.SendSchedule(content, mailsentTO, Username, subject, AdminEmail, SMTPEmail, SMTPPwd, SMPTPort, SMTPMail);
                        if (res == "MailSent")
                        {
                            return new APIResponse(true, "", "Schedule Updated Successfully and Mail Sent Successfully!");

                        }
                        else
                        {
                            return new APIResponse(true, res, "Schedule Updated Successfully and Mail Not Sent !");

                        }
                    }
                    else
                    {
                        return new APIResponse(true, "", "Schedule Updated Successfully!");
                    }
                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", ex.Message);
            }

        }

    }

}
