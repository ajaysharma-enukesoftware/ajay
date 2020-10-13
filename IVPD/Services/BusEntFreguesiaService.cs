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
    public interface IBusEntFreguesiaService
    {
        public List<BusEntFreguesia> GetAll();
        public List<BusEntFreguesia> GetByID(int id);
    }
    public class BusEntFreguesiaService : IBusEntFreguesiaService
    {
        private IVPDContext _context;

        public BusEntFreguesiaService(IVPDContext context)
        {
            _context = context;
        }
        public List<BusEntFreguesia> GetAll()
        {
            List<BusEntFreguesia> alldata = _context.Freguesia.OrderBy(o=>o.DESFRG).ToList();
            return alldata;
        }

        public List<BusEntFreguesia> GetByID(int id)
        {
            List<BusEntFreguesia> alldata = _context.Freguesia.Where(w => Convert.ToInt32(w.CODCON) == id).OrderBy(o => o.DESFRG).ToList();
            return alldata;
        }
    }
}
