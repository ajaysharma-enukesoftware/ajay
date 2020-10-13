
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
    public interface IPaymentReturnedService
    {
        public List<PaymentReturned> GetAll();
    }
    public class PaymentReturnedService:IPaymentReturnedService
    {
        private IVPDContext _context;

        public PaymentReturnedService(IVPDContext context)
        {
            _context = context;
        }
        public List<PaymentReturned> GetAll()
        {
            //List<PaymentReturned> datas= _context.PaymentReturneds.ToList();
            List<PaymentReturned> datas = new List<PaymentReturned>();
            PaymentReturned obj = new PaymentReturned();
            obj.Base = "2021";
            obj.Data_pagamento = "2008";
            obj.nif = "1025478";
            obj.nome = "1";
            obj.N_Carta = "Replanting right with prior start";
            obj.n_transferencia = 1;
            obj.Observacoes = "2009";
            obj.Outros_pagamentos = "12";
            obj.Preco_pipa = "123";
            obj.Qnt = "2019";
            obj.Tipo_Prd = "Testing";
            obj.n_viticultor = "Test";
            obj.Valor_ja_Prago = "Checking";
            obj.Valor = "Checking";
            obj.tipo_production = "Checking";
            datas.Add(obj);
            PaymentReturned obj1 = new PaymentReturned();
            obj1.Base = "2011";
            obj1.Data_pagamento = "2018";
            obj1.nif = "15478";
            obj1.nome = "1";
            obj1.N_Carta = "Replanting right with prior start 112";
            obj1.n_transferencia = 2;
            obj1.Observacoes = "2019";
            obj1.Outros_pagamentos = "112";
            obj1.Preco_pipa = "12387";
            obj1.Qnt = "2009";
            obj1.Tipo_Prd = "Testing 123";
            obj1.n_viticultor = "Test 123";
            obj1.Valor_ja_Prago = "Checking 123";
            obj1.Valor = "Checking 123";
            obj1.tipo_production = "Checking 123";
            datas.Add(obj1);
            return datas;
        }
    }
}
