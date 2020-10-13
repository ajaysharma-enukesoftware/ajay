using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IOutstandingPaymentService
    {
        public List<OutstandingPayment> GetAll();
    }
    public class OutstandingPaymentService: IOutstandingPaymentService
    {
        private IVPDContext _context;

        public OutstandingPaymentService(IVPDContext context)
        {
            _context = context;
        }
        public List<OutstandingPayment> GetAll()
        {
            //List<RecocileProducer> datas= _context.RecocileProducers.ToList();
            List<OutstandingPayment> datas = new List<OutstandingPayment>();
            OutstandingPayment obj = new OutstandingPayment();
            obj.Base = "1";
            obj.Ent_que_paga = "Lady of the Rosary";
            obj.Ent_que_recebe = "2009";
            obj.nib = "648";
            obj.nif = "DL 504-I";
            obj.n_entidade = "2021";
            obj.n_entidade2 = "Replanting right with prior start";
            obj.n_ficheiro = "648";
            obj.Outros_pagamentos = "2008";
            obj.Preco_pipa = "1025478";
            obj.Qnt = "1025478";
            obj.Tipo_Prd = "1025478";
            obj.Valor_ja_Prago = "dsds";
            

            datas.Add(obj);
            OutstandingPayment obj1 = new OutstandingPayment();
            obj1.Base = "2";
            obj1.Ent_que_paga = "Lady of the Rosary";
            obj1.Ent_que_recebe = "2009";
            obj1.nib = "648";
            obj1.nif = "DL 504-I";
            obj1.n_entidade = "2021";
            obj1.n_entidade2 = "Replanting right with prior start";
            obj1.n_ficheiro = "648";
            obj1.Outros_pagamentos = "2008";
            obj1.Preco_pipa = "1025478";
            obj1.Qnt = "1025478";
            obj1.Tipo_Prd = "1025478";
            obj1.Valor_ja_Prago = "dsds";

            datas.Add(obj1);
            return datas;
        }
    }
}
