using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
   
        public interface IPendingPaymentService
        {
            public List<PendingPayment> GetAll();
        }
        public class PendingPaymentService : IPendingPaymentService
    {
            private IVPDContext _context;

            public PendingPaymentService(IVPDContext context)
            {
                _context = context;
            }
            public List<PendingPayment> GetAll()
            {
                //List<RecocileProducer> datas= _context.RecocileProducers.ToList();
                List<PendingPayment> datas = new List<PendingPayment>();
            PendingPayment obj = new PendingPayment();
                obj.ano = "1";
                obj.Base = "Lady of the Rosary";
                obj.Entidade_que_paga = "2009";
                obj.Entidade_que_recebe = "648";
                obj.estado = "648";
                obj.nib = "648";
                obj.nif = "648";
                obj.n_entidade = "648";
                obj.tipo_prd = "648";
                obj.valor_a_pagar = "648";


            datas.Add(obj);
            PendingPayment obj1 = new PendingPayment();
            obj1.ano = "2";
            obj1.Base = "Lady ";
            obj1.Entidade_que_paga = "34";
            obj1.Entidade_que_recebe = "34";
            obj1.estado = "34";
            obj1.nib = "34";
            obj1.nif = "34";
            obj1.n_entidade = "34";
            obj1.tipo_prd = "34";
            obj1.valor_a_pagar = "34";
            datas.Add(obj1);
                return datas;
            }

        }

    
}
