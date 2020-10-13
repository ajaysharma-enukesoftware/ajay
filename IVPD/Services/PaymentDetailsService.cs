using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IPaymentDetailsService
    {
        public List<PaymentDetails> GetAll();
    }
    public class PaymentDetailsService : IPaymentDetailsService
    {

        private IVPDContext _context;

        public PaymentDetailsService(IVPDContext context)
        {
            _context = context;
        }
        public List<PaymentDetails> GetAll()
        {
            //List<RecocileProducer> datas= _context.RecocileProducers.ToList();
            List<PaymentDetails> datas = new List<PaymentDetails>();
            PaymentDetails obj = new PaymentDetails();
            obj.ficheiro = "1";
            obj.Base = "2009";
            obj.Entr = "648";
            obj.n_ficheiro = "DL 504-I";
            obj.Tipo_Prd = "2021";
            obj.valor_a_pagar = "winter is coming";

            datas.Add(obj);
            PaymentDetails obj1 = new PaymentDetails();
            obj1.ficheiro = "1";
            obj1.Base = "2009";
            obj1.Entr = "648";
            obj1.n_ficheiro = "DL 504-I";
            obj1.Tipo_Prd = "2021";
            obj1.valor_a_pagar = "white walkers";


            datas.Add(obj1);
            return datas;
        }
    }
}
