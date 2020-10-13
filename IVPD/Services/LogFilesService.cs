
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Models;
using IVPD.Helpers;
using Microsoft.EntityFrameworkCore;
using Audit.Core;

namespace IVPD.Services
{
    public interface ILogFilesService
    {
        public List<LogFiles> GetAll();
    }
    public class LogFilesService:ILogFilesService
    {
        private IVPDContext _context;

        public LogFilesService(IVPDContext context)
        {
            _context = context;
        }
        public List<LogFiles> GetAll()
        {
            //List<LogFiles> datas= _context.LogFiless.ToList();
            List<LogFiles> datas = new List<LogFiles>();
            LogFiles obj = new LogFiles();
            obj.N_entidade =1;
            obj.entidade = "2008";
            obj.Valor_total = "2009";
            datas.Add(obj);
            LogFiles obj1 = new LogFiles();
            obj1.N_entidade = 2;
            obj1.entidade = "2018";
            obj1.Valor_total = "2019";
            datas.Add(obj1);
            return datas;
        }
    }
}
