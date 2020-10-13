using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IConsultCurrentAccountService
    {
       // public IEnumerable<ConsultCurrentAccount> GetAllPreviousMovements(int txtNUMENT, string dateInitial);
        public List<ConsultCurrentAccount> GetAllPeriodMovements(long txtNUMENT, string dateInicial, string dateFinal);

        public bool checkInitialDocs(string dateInitial);
    }
    public class ConsultCurrentAccountService : IConsultCurrentAccountService
    {
        private RevenueContext _context;
        public ConsultCurrentAccountService(RevenueContext context)
        {
            _context = context;
        }
        public bool checkInitialDocs(string dateInitial)
        {
            //   DateTime dt = Convert.ToDateTime(dateInitial);

            //Know the year
            string str = dateInitial.Substring(0, 4);

            int year =Convert.ToInt32(str);
            var ANODOCdata = _context.LIBTESOUR_THDOCUF.Where(x => x.ANODOC >= year).First();
            if (ANODOCdata != null)
            {
                return true;
            }
            else
            {
                return false;
            }   
        }
       /* public void OpeningBalance(int txtNUMENT, string dateInitial)
        {
            decimal dAcum = 0;
            var a = _context.LIBTESOUR_TSSALDF.Where(x => x.VITSAL.CompareTo(txtNUMENT.ToString()) > 0 ).First();
            if (a != null)
            {
                if (checkInitialDocs( dateInitial))
                {
                    dAcum = dAcum + Convert.ToDecimal(a.INISAL);
                }
                else
                {
                    dAcum = dAcum + Convert.ToDecimal(a.INISAL) + Convert.ToDecimal(a.ARQSAL);

                }
            }
           
        }*/
        public IEnumerable<ConsultCurrentAccount> GetAllPreviousMovements(int txtNUMENT, string dateInitial)
        {
          int TDOC_ANULADO = -1;
            if(checkInitialDocs(dateInitial))
            {
string query = "SELECT SUM(c.TOTDOC*c.FMCDOC) AS ACUM FROM LIBTESOUR_THDOCUF c LEFT JOIN LIBTESOUR_TSTIPDF t ON t.CODTIP = c.TIPDOC AND t.ANOTIP = c.ANODOC WHERE c.ESTDOC <> " + TDOC_ANULADO + " AND c.FMCDOC <> 0 AND ((c.VT2DOC = '" + txtNUMENT + "' AND t.DOCTIP <> 'TPDOC_RA') OR (c.VITDOC = '" + txtNUMENT + "' AND t.DOCTIP = 'TPDOC_RA')) AND c.DATDOC < " + dateInitial;

                                // date without -,/
                var result = _context.ConsultCurrentAccount
                                    .FromSqlRaw(query)
                                    .ToList();
                return result;

            }
            else
            {
                string query = "SELECT SUM(c.TOTDOC*c.FMCDOC) AS ACUM FROM LIBTESOUR_THDOCUF c LEFT JOIN LIBTESOUR_TSTIPDF t ON t.CODTIP = c.TIPDOC AND t.ANOTIP = c.ANODOC WHERE c.ESTDOC <> " + TDOC_ANULADO + " AND c.FMCDOC <> 0 AND ((c.VT2DOC = '" + txtNUMENT + "' AND t.DOCTIP <> 'TPDOC_RA') OR (c.VITDOC = '" + txtNUMENT + "' AND t.DOCTIP = 'TPDOC_RA')) AND c.DATDOC < " + dateInitial;
                var result = _context.ConsultCurrentAccount
                                 .FromSqlRaw(query)
                                 .ToList();
                return result;
            }

        }
        public List<ConsultCurrentAccount> GetAllPeriodMovements(long txtNUMENT, string dateInicial, string dateFinal)
        {
          //  DateTime dt = DateTime.ParseExact(dateInicial, "yyyy/MM/dd", CultureInfo.InvariantCulture);
       //     DateTime datei = dt.Date;
            int TDOC_ANULADO = -1;
                if (checkInitialDocs(dateInicial))
                {
                    string query = "SELECT c.ANODOC, c.TIPDOC" +
                        ", c.NUMDOC, c.DATDOC, c.TOTDOC, c.FMCDOC, c.SVADOC, c.IVADOC, t.DOCTIP, t.DESTIP, t.SINTIP, c.INSDOC, c.HORDOC, c.UTIDOC" +
                        " FROM LIBTESOUR_THDOCUF c" +
                       " LEFT JOIN LIBTESOUR_TSTIPDF t ON t.CODTIP = c.TIPDOC AND t.ANOTIP = c.ANODOC" +
                        " WHERE c.ESTDOC <> " + TDOC_ANULADO + " AND c.FMCDOC <> 0" +
                        " AND ((c.VT2DOC = '" + txtNUMENT + "' AND t.DOCTIP <> 'TPDOC_RA') OR (c.VITDOC = '" + txtNUMENT + "' AND t.DOCTIP = 'TPDOC_RA'))" +
                        " AND c.DATDOC >= " + Convert.ToDecimal(dateInicial) +
                        " AND c.DATDOC <= " + Convert.ToDecimal(dateFinal);

                    // date without -,/
                    string query2 = "UNION " +
                         "SELECT c.ANODOC, c.TIPDOC" +
                         ", c.NUMDOC, c.DATDOC, c.TOTDOC, c.FMCDOC, c.SVADOC, c.IVADOC, t.DOCTIP, t.DESTIP, t.SINTIP, c.INSDOC, c.HORDOC, c.UTIDOC" +
                         " FROM LIBTESOUR_TSDOCUF c" +
                         " LEFT JOIN LIBTESOUR_TSTIPDF t ON t.CODTIP = c.TIPDOC AND t.ANOTIP = c.ANODOC" +
                         " WHERE c.ESTDOC <> " + TDOC_ANULADO + " AND c.FMCDOC <> 0" +
                         " AND ((c.VT2DOC = '" + txtNUMENT + "' AND t.DOCTIP <> 'TPDOC_RA') OR (c.VITDOC = '" + txtNUMENT + "' AND t.DOCTIP = 'TPDOC_RA'))" +
                         " AND c.DATDOC >= " + Convert.ToDecimal(dateInicial) +
                        " AND c.DATDOC <= " + Convert.ToDecimal(dateFinal) +
                        " ORDER BY DATDOC, INSDOC, HORDOC";

                    string query3 = query + query2;
                List<ConsultCurrentAccount> result = _context.ConsultCurrentAccount
                                    .FromSqlRaw(query3).ToList();
                                        
                    if (result != null) { return result; }
                    else { return null; }
             
            }
            else
            {
                string query = "  SELECT c.ANODOC, c.TIPDOC"
                         + ", c.NUMDOC, c.DATDOC, c.TOTDOC, c.FMCDOC, c.SVADOC, c.IVADOC, t.DOCTIP, t.DESTIP, t.SINTIP, c.INSDOC, c.HORDOC, c.UTIDOC"
                         + " FROM LIBTESOUR_TSDOCUF c"
                         + " LEFT JOIN LIBTESOUR_TSTIPDF t ON t.CODTIP = c.TIPDOC AND t.ANOTIP = c.ANODOC"
                         + " WHERE c.ESTDOC <> " + TDOC_ANULADO + " AND c.FMCDOC <> 0"
                         + " AND ((c.VT2DOC = '" + txtNUMENT + "' AND t.DOCTIP <> 'TPDOC_RA') OR (c.VITDOC = '" + txtNUMENT + "' AND t.DOCTIP = 'TPDOC_RA'))"
                         + " AND c.DATDOC >=  " + Convert.ToDecimal(dateInicial)
                      + " AND c.DATDOC <= " + Convert.ToDecimal(dateFinal)
                      + " ORDER BY c.DATDOC, c.INSDOC, c.HORDOC";
                List<ConsultCurrentAccount> result = _context.ConsultCurrentAccount
                                 .FromSqlRaw(query).ToList()
                                ;
                if (result != null) { return result; }
                else { return null; }
            }

        }
    }
}
