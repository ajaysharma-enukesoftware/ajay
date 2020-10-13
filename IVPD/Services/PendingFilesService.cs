
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
    public interface IPendingFilesService
    {
        public List<PendingFiles> GetAll();
    }
    public class PendingFilesService:IPendingFilesService
    {
        private IVPDContext _context;

        public PendingFilesService(IVPDContext context)
        {
            _context = context;
        }
        public List<PendingFiles> GetAll()
        {
            //List<PendingFiles> datas= _context.PendingFiless.ToList();
            List<PendingFiles> datas = new List<PendingFiles>();
            PendingFiles obj = new PendingFiles();
            obj.entidade = "2021";
            obj.estado = "2008";
            obj.ficheiro = "1025478";
            obj.n_ficheiro = "1";
            obj.n_movimento = "Replanting right with prior start";
            obj.origem = "8745";
            obj.valor_retido = "2009";
            obj.valor_total = "12";
            datas.Add(obj);
            PendingFiles obj1 = new PendingFiles();
            obj1.entidade = "2011";
            obj1.estado = "2018";
            obj1.ficheiro = "1478";
            obj1.n_ficheiro = "2";
            obj1.n_movimento = "Replanting right with prior start 123";
            obj1.origem = "874514";
            obj1.valor_retido = "2019";
            obj1.valor_total = "127";
            datas.Add(obj1);
            return datas;
        }
    }
}
