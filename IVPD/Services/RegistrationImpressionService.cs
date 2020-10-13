using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IRegistrationImpressionService
    {
        public IEnumerable<RegistrationImpression> GetAll();
    }
    public class RegistrationImpressionService: IRegistrationImpressionService
    {
        private VindimaContext _context;

        public RegistrationImpressionService(VindimaContext context)
        {
            _context = context;
        }
        public IEnumerable<RegistrationImpression> GetAll()
        {
            /* var result = (from r in _context.REGCARTAS
                             join e in _context.ENTIDADES on r.RCBDES equals e.ENTNUM
                             join l in _context.FRBLIN on r.RCBNCA equals l.FBLNCA into RegistrationImpression

                             from m in RegistrationImpression.DefaultIfEmpty()
                             select new RegistrationImpression()
                             {
                                 RCBNUM = r.RCBNUM,
                                 RCBDES = r.RCBDES,
                                 ENTNOM = e.ENTNOM,
                                 RCBNCA = r.RCBNCA,
                                 RCBDAT = r.RCBDAT,
                                 RCBUSR = r.RCBUSR,
                                 RCBWKS = r.RCBWKS,
                                 RCBSTS = r.RCBSTS,
                                 RCBDT2 = r.RCBDT2,
                                 RCBHR2 = r.RCBHR2,
                                 RCBUR2 = r.RCBUR2,
                                 RCBWS2 = r.RCBWS2,
                                 RCBMOT = r.RCBMOT,
                                 FBLSTS = m.FBLSTS

                             });
               return result;*/
            string query;
           
query = "SELECT r.RCBNUM, r.RCBDES, e.ENTNOM, r.RCBNCA, r.RCBDAT, r.RCBUSR, r.RCBWKS, r.RCBSTS, r.RCBDT2, r.RCBHR2, r.RCBUR2, r.RCBWS2, r.RCBMOT, l.FBLSTS FROM (REGCARTAS r LEFT JOIN ENTIDADES e ON e.ENTNUM   = r.RCBDES   ) LEFT JOIN FRBLIN l ON l.FBLNCA  COLLATE DATABASE_DEFAULT  = r.RCBNCA  COLLATE DATABASE_DEFAULT  ORDER BY r.RCBNUM DESC";

           
            var EntityList = _context.RegistrationImpression
                    .FromSqlRaw(query)
                    .ToList();

            return EntityList;
        }
    }
}
