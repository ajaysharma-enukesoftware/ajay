using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{

    public interface IBusEntConcelhoService
    {
        public List<BusEntConcelho> BusEntConcelhoGetAll();
        public List<BusEntConcelho> BusEntConcelhoGetByID(int id);
        public List<BusEntConcelho> BusEntConcelhoGetByName(string descon);

    }
    public class BusEntConcelhoService : IBusEntConcelhoService
    {
        private IVPDContext _context;

        public BusEntConcelhoService(IVPDContext context)
        {
            _context = context;
        }

        public List<BusEntConcelho> BusEntConcelhoGetAll()
        {
            return _context.CONCELHO.AsNoTracking().OrderBy(o => o.Descon).ToList();
        }

        public List<BusEntConcelho> BusEntConcelhoGetByID(int id)
        {
            return _context.CONCELHO.AsNoTracking().Where(w => Convert.ToInt32(w.Coddis) == id).OrderBy(o => o.Descon).ToList();
        }
        public List<BusEntConcelho> BusEntConcelhoGetByName(string descon)
        {
            return _context.CONCELHO.AsNoTracking().Where(o => o.Descon == descon).ToList();
        }

    }
}
