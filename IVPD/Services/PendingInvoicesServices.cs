using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IPendingInvoicesServices
    {
       // public IEnumerable<PendingInvoices> GetAlls();

    }
    public class PendingInvoicesServices : IPendingInvoicesServices
    {
        private RevenueContext _context;

        public PendingInvoicesServices(RevenueContext context)
        {
            _context = context;
        }
      /*  public List<PendingInvoices> GetAllPendingInvoice(long codEntidade)
        {
           long m_codEntidade=codEntidade;
           int FT_PENDENTE = 0, TPDOC_FT=0;
           string query="SELECT d.ANONCC, d.TIPNCC, d.NUMNCC, d.DATNCC, d.VPANCC, d.TOTNCC, d.IVANCC" 
                        +", ROUND(d.TOTNCC-(d.TOTNCC*d.VPANCC/100), 2) AS VEFNCC, ROUND(d.IVANCC-(d.IVANCC*d.VPANCC/100), 2) AS VEFIVA, d.PRZNCC" 
                        +", d.DTLNCC, d.QTDNCC, d.INSNCC, d.HORNCC, d.SINNCC, e.ENTNUM, e.ENTNOM, d.MORDOC" 
                        +" FROM LIBTESOUR_TSDNCCF d" 
                        +" LEFT JOIN ENTIDADES_ENTIVDPF e ON e.ENTNUM = d.VITNCC" 
                        +" WHERE d.ESTNCC = " + FT_PENDENTE + " AND d.TIPNCC = " + TPDOC_FT+"";
   
            if(m_codEntidade > 0) {
                string query2 = query + " AND e.ENTNUM = '" + m_codEntidade + " ORDER BY d.INSNCC DESC, d.HORNCC DESC";
            // date without -,/
            var result = _context.PendingInvoices
                                .FromSqlRaw(query2)
                                .ToList();
            return result;

        }*/

    }
}
