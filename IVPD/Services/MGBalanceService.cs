using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace IVPD.Services
{
    public interface IMGBalanceService
    {
        public IEnumerable<MGBalance> GetAll();
    }
    public class MGBalanceService: IMGBalanceService
    {
        private VindimaContext _context;
        public MGBalanceService(VindimaContext context)
        {
            _context = context;
        }
        public IEnumerable<MGBalance> GetAll()
        {

            var result = (from p in _context.EMTPMG
                          join e in _context.ENTIDADES on p.PMGENT equals e.ENTNUM into MGBalance
                          from m in MGBalance.DefaultIfEmpty()
                          select new MGBalance()
                          {
                              PMGENT = p.PMGENT,
                              ENTNOM = m.ENTNOM,
                              PMGPMB = p.PMGPMB,
                              PMGPMT = p.PMGPMT,
                              PMGPMR = p.PMGPMR,
                              PMGCMB = p.PMGCMB,
                              PMGCMT = p.PMGCMT,
                              PMGCMR = p.PMGCMR,
                              PMGADB = p.PMGADB,
                              PMGADT = p.PMGADT,
                              PMGADR = p.PMGADR,
                              PMGTCA = p.PMGTCA,
                              TVINHO = p.PMGPMB + p.PMGPMT + p.PMGPMR + p.PMGCMB + p.PMGCMT + p.PMGCMR + p.PMGADB + p.PMGADT + p.PMGADR + p.PMGTCA,
                              PMGTVD = p.PMGTVD,
                              PMGSAL = p.PMGPMB + p.PMGPMT + p.PMGPMR + p.PMGCMB + p.PMGCMT + p.PMGCMR + p.PMGADB + p.PMGADT + p.PMGADR + p.PMGTCA - p.PMGTVD

                          });
           return result;
        }
    }
}
