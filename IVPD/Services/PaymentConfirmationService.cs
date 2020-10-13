using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IPaymentConfirmationService
    {
        public List<PaymentConfirmation> GetAll();
    }
    public class PaymentConfirmationService: IPaymentConfirmationService
    {
        private IVPDContext _context;

        public PaymentConfirmationService(IVPDContext context)
        {
            _context = context;
        }
        public List<PaymentConfirmation> GetAll()
        {
            //List<RecocileProducer> datas= _context.RecocileProducers.ToList();
            List<PaymentConfirmation> datas = new List<PaymentConfirmation>();
            PaymentConfirmation obj = new PaymentConfirmation();
            obj.carta = "1";
            obj.data_pagamento = "test";
            obj.entidade_beneficiaria = "2009";
            obj.entidade_pagadora = "648";
            obj.n_entidade = "DL 504-I";
            obj.n_ficheiro = "2021";
            obj.valor = "vip";
           
            datas.Add(obj);
            PaymentConfirmation obj1 = new PaymentConfirmation();
            obj1.carta = "1";
            obj1.data_pagamento = "testing";
            obj1.entidade_beneficiaria = "2009";
            obj1.entidade_pagadora = "648";
            obj1.n_entidade = "DL 504-I";
            obj1.n_ficheiro = "2021";
            obj1.valor = "vipul";

            datas.Add(obj1);
            return datas;
        }
    }
}
