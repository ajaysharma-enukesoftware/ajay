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
    public interface IImportedPaymentFilesService
    {
        public List<ImportedPaymentFiles> GetAll();
    }
    public class ImportedPaymentFilesService:IImportedPaymentFilesService
    {
        private IVPDContext _context;

        public ImportedPaymentFilesService(IVPDContext context)
        {
            _context = context;
        }
        public List<ImportedPaymentFiles> GetAll()
        {
            //List<ImportedPaymentFiles> datas= _context.ImportedPaymentFiless.ToList();
            List<ImportedPaymentFiles> datas = new List<ImportedPaymentFiles>();
            ImportedPaymentFiles obj = new ImportedPaymentFiles();
            obj.entidade = "Replanting right with prior start";
            obj.estado = "Test 1";
            obj.ficheiro = "Test 2";
            obj.n_ficheiro = "1";
            obj.n_rec = "Test 3";
            obj.valor_total = "Test 4";
            datas.Add(obj);
            ImportedPaymentFiles obj1 = new ImportedPaymentFiles();
            obj1.entidade = "Replanting right with prior start 123";
            obj1.estado = "Testing 1";
            obj1.ficheiro = "Testing 2";
            obj1.n_ficheiro = "2";
            obj1.n_rec = "Testing 3";
            obj1.valor_total = "Testing 4";
            datas.Add(obj1);
            return datas;
        }
    }
}
