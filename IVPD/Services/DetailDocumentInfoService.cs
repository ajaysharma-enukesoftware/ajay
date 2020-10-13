using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IDetailDocumentInfoService
    {
        // public IEnumerable<ConsultCurrentAccount> GetAllPreviousMovements(int txtNUMENT, string dateInitial);
        //  public List<DetailDocumentInfo> GetAllPeriodMovements(long txtNUMENT, string dateInicial, string dateFinal);
        public List<DetailDocumentInfo> GetAll();
        public DetailDocumentInfo GetInfo();

    }
    public class DetailDocumentInfoService: IDetailDocumentInfoService
    {
        private RevenueContext _context;
        public DetailDocumentInfoService(RevenueContext context)
        {
            _context = context;
        }
        public List<DetailDocumentInfo> GetAll()
        {
            int m_ANO=2003, m_TIP=0, m_NUM=1;

          string  sQuery = "SELECT l.LINDOL, l.QTDDOL, l.UNIDOL, l.VALDOL, l.IVADOL, l.TXAIVA, l.REFDOL, l.DESDOL, l.CTBDOL, l.DTEDOL, i.DESIVA" 
                      + " FROM LIBTESOUR_TSDOCLF l" 
                      + " LEFT JOIN LIBTESOUR_TSCIVAF i ON i.VALIVA = l.TXAIVA" 
                      + " WHERE l.ANODOL = " + m_ANO 
                      + " AND l.TIPDOL = " + m_TIP 
                      + " AND l.NUMDOL = " + m_NUM 
                      + " ORDER BY l.LINDOL";
            List<DetailDocumentInfo> result = _context.DetailDocumentInfo
                                 .FromSqlRaw(sQuery)
                                 .ToList();
                return result;
            

        }
        public DetailDocumentInfo GetInfo()
        {
            int m_ANO = 2003, m_TIP = 0, m_NUM = 1;

            string sQuery = "SELECT h.DATDOC, h.IVADOC, h.TOTDOC, h.QTDDOC, h.ESTDOC, e.ENTNUM, e.ENTNOM" 
            + " FROM LIBTESOUR_TSDOCUF h" 
            + " LEFT JOIN ENTIDADES_ENTIVDPF e ON e.ENTNUM = h.VITDOC" 
            + " WHERE h.ANODOC = "+ m_ANO 
            + " AND h.TIPDOC = " + m_TIP 
            + " AND h.NUMDOC = " + m_NUM;
            DetailDocumentInfo result = _context.DetailDocumentInfo
                                 .FromSqlRaw(sQuery)
                                 .FirstOrDefault();
            if (result != null) { return result; }
            else { return null; }
            


        }
    }
}
