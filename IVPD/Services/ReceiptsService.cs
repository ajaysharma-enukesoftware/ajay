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
    public interface IReceiptsService
    {
        public List<Receipts> GetAll();
    }
    public class ReceiptsService:IReceiptsService
    {
        private IVPDContext _context;

        public ReceiptsService(IVPDContext context)
        {
            _context = context;
        }
        public List<Receipts> GetAll()
        {
            //List<Receipts> datas= _context.Receiptss.ToList();
            List<Receipts> datas = new List<Receipts>();
            Receipts obj = new Receipts();
            obj.ano = "2021";
            obj.Base = "2008";
            obj.data_transf = "1025478";
            obj.destinatario = "Test";
            obj.entidade_pag = "Replanting right with prior start";
            obj.ent_rec = "8745";
            obj.nif_dest = "2009";
            obj.nif_pag = "2009";
            obj.nif_rec = "2009";
            obj.n_carta = "2009";
            obj.n_ficha = "2009";
            obj.Outros_pag = "2009";
            obj.Preco_Pipa = "2009";
            obj.Qtd = "2009";
            obj.tipo_pag = "2009";
            obj.Tipo_Prd = "2009";
            obj.valor_a_pagar = "2009";
            obj.valor_ja_pago = "2009";
            obj.v_retido = "2009";
            datas.Add(obj);
            Receipts obj1 = new Receipts();
            obj1.ano = "2011";
            obj1.Base = "2018";
            obj1.data_transf = "1478";
            obj1.destinatario = "Test 123";
            obj1.entidade_pag = "Replanting right with prior start 147";
            obj1.ent_rec = "877445";
            obj1.nif_dest = "Test";
            obj1.nif_pag = "Test 1";
            obj1.nif_rec = "Test 2";
            obj1.n_carta = "Test 3";
            obj1.n_ficha = "Test 4";
            obj1.Outros_pag = "Test 5";
            obj1.Preco_Pipa = "Test 6";
            obj1.Qtd = "Test 7";
            obj1.tipo_pag = "Test 8";
            obj1.Tipo_Prd = "Test 9";
            obj1.valor_a_pagar = "Test 10";
            obj1.valor_ja_pago = "Test 11";
            obj1.v_retido = "Test 12";
            datas.Add(obj1);
            return datas;
        }
    }
}
