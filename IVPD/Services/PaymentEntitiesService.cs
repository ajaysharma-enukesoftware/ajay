
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
    public interface IPaymentEntitiesService
    {
        public List<PaymentEntities> GetAll();
    }
    public class PaymentEntitiesService:IPaymentEntitiesService
    {
        private IVPDContext _context;

        public PaymentEntitiesService(IVPDContext context)
        {
            _context = context;
        }
        public List<PaymentEntities> GetAll()
        {
            //List<PaymentEntities> datas= _context.PaymentEntitiess.ToList();
            List<PaymentEntities> datas = new List<PaymentEntities>();
            PaymentEntities obj = new PaymentEntities();
            obj.entidade = "2021";
            obj.valor_total = "2008";
            obj.Quantidade_total = "1025478";
            obj.Produto = "Test";
            obj.n_entidade = "Replanting right with prior start";
            obj.Nif = "8745";
            obj.Base = "2009";
            datas.Add(obj);
            PaymentEntities obj1 = new PaymentEntities();
            obj1.entidade = "2011";
            obj1.valor_total = "2018";
            obj1.Quantidade_total = "1478";
            obj1.Produto = "Test 2";
            obj1.n_entidade = "Replanting right with prior start 123";
            obj1.Nif = "877845";
            obj1.Base = "2019";
            datas.Add(obj1);
            return datas;
        }
    }
}
