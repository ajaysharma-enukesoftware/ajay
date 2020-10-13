using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IPaymentFileDetailService
    {
        public List<PaymentFileDetail> GetAll();
    }
    public class PaymentFileDetailService: IPaymentFileDetailService
    {
       
            private IVPDContext _context;

            public PaymentFileDetailService(IVPDContext context)
            {
                _context = context;
            }
            public List<PaymentFileDetail> GetAll()
            {
                //List<RecocileProducer> datas= _context.RecocileProducers.ToList();
                List<PaymentFileDetail> datas = new List<PaymentFileDetail>();
                PaymentFileDetail obj = new PaymentFileDetail();
                obj.data_pag = "1";
                obj.Entr = "Lady of the Rosary";
                obj.estado = "2009";
                obj.nome_ficheiro = "648";
                obj.n_vit = "DL 504-I";
                obj.outros_pag = "2021";
                obj.quant = "Replanting right with prior start";
                obj.Tipo_Prd = "648";
                obj.valor_a_pagar = "2008";
                obj.v_pago = "1025478";
                obj.v_pipa = "1025478";
            

                datas.Add(obj);
            PaymentFileDetail obj1 = new PaymentFileDetail();
            obj1.data_pag = "2";
            obj1.Entr = "Lady of the vistros";
            obj1.estado = "got";
            obj1.nome_ficheiro = "648";
            obj1.n_vit = "DL 504-I";
            obj1.outros_pag = "2021";
            obj1.quant = "kings landing";
            obj1.Tipo_Prd = "648";
            obj1.valor_a_pagar = "2008";
            obj1.v_pago = "1025478";
            obj1.v_pipa = "1025478";


            datas.Add(obj1);
                return datas;
            }
        }

    
}
