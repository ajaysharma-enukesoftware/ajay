using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IPaymentNotSentService
    {
        public List<PaymentNotSent> GetAll();
    }
    public class PaymentNotSentService: IPaymentNotSentService
    {
        private IVPDContext _context;

        public PaymentNotSentService(IVPDContext context)
        {
            _context = context;
        }
        public List<PaymentNotSent> GetAll()
        {
            //List<RecocileProducer> datas= _context.RecocileProducers.ToList();
            List<PaymentNotSent> datas = new List<PaymentNotSent>();
            PaymentNotSent obj = new PaymentNotSent();
            obj.data_ficheiro = "1";
            obj.Estado = "Lady of the Rosary";
            obj.montante = "2009";
            obj.n_movimento = "648";
            

            datas.Add(obj);
            PaymentNotSent obj1 = new PaymentNotSent();
            obj1.data_ficheiro = "2";
            obj1.Estado = "Lady of the Rosary";
            obj1.montante = "2009";
            obj1.n_movimento = "648";
            datas.Add(obj1);
            return datas;
        }

    }
}
