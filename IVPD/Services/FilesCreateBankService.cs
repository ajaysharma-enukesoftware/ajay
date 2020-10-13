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
    public interface IFilesCreateBankService
    {
        public List<FilesCreateBank> GetAll();
    }
    public class FilesCreateBankService:IFilesCreateBankService
    {
        private IVPDContext _context;

        public FilesCreateBankService(IVPDContext context)
        {
            _context = context;
        }
        public List<FilesCreateBank> GetAll()
        {
            //List<FilesCreateBank> datas= _context.FilesCreateBanks.ToList();
            List<FilesCreateBank> datas = new List<FilesCreateBank>();
            FilesCreateBank obj = new FilesCreateBank();
            obj.entidade = "Replanting right with prior start";
            obj.estado = "Test 1";
            obj.n_ficheiro = "1";
            obj.n_rec = "Test 3";
            obj.valor_total = "Test 4";
            obj.dgt = "Test 5";
            obj.n_tei = "Test 6";
            obj.data_hora = "Test 7";
            obj.utilizador = "Test 8";
            datas.Add(obj);
            FilesCreateBank obj1 = new FilesCreateBank();
            obj1.entidade = "Replanting right with prior start 123";
            obj1.estado = "Testing 1";
            obj1.n_ficheiro = "2";
            obj1.n_rec = "Testing 3";
            obj1.valor_total = "Testing 4";
            obj1.dgt = "Testing 5";
            obj1.n_tei = "Testing 6";
            obj1.data_hora = "Testing 7";
            obj1.utilizador = "Testing 8";
            datas.Add(obj1);
            return datas;
        }
    }
}
