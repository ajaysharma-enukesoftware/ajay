using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IPaymentHeldService
    {
        public List<PaymentHeld> GetAll();
    }
    public class PaymentHeldService: IPaymentHeldService
    {
        private IVPDContext _context;

        public PaymentHeldService(IVPDContext context)
        {
            _context = context;
        }
        public List<PaymentHeld> GetAll()
        {
            //List<RecocileProducer> datas= _context.RecocileProducers.ToList();
            List<PaymentHeld> datas = new List<PaymentHeld>();
            PaymentHeld obj = new PaymentHeld();
            obj.Base = "11";
            obj.Ent_que_paga = "Lady of the Rosary";
            obj.Ent_recebe = "2009";
            obj.Estado = "648";
            obj.Motivo = "1";
            obj.n_entidade = "Lady of the Rosary";
            obj.n_ficheiro = "2009";
            obj.Outros_pagamentos = "648";
            obj.Preco_pipa = "648";
            obj.Qnt = "648";
            obj.Tipo_Prd = "648";
            obj.valor_a_pagar = "648";
            obj.Valor_ja_Prago = "648";
            obj.valor_retido = "648";
          



            datas.Add(obj);
            PaymentHeld obj1 = new PaymentHeld();
            obj1.Base = "2";
            obj1.Ent_que_paga = "Lady of the Rosary";
            obj1.Ent_recebe = "2009";
            obj1.Estado = "648";
            obj1.Motivo = "1";
            obj1.n_entidade = "Lady of the Rosary";
            obj1.n_ficheiro = "2009";
            obj1.Outros_pagamentos = "648";
            obj1.Preco_pipa = "648";
            obj1.Qnt = "648";
            obj1.Tipo_Prd = "648";
            obj1.valor_a_pagar = "648";
            obj1.Valor_ja_Prago = "648";
            obj1.valor_retido = "648";
            datas.Add(obj1);
            return datas;
        }

    }
}
