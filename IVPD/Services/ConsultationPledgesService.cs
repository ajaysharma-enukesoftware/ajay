using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IConsultationPledgesService
    {
        public List<ConsultationPledges> GetAll();
    }
    public class ConsultationPledgesService: IConsultationPledgesService
    {
        private IVPDContext _context;

        public ConsultationPledgesService(IVPDContext context)
        {
            _context = context;
        }
        public List<ConsultationPledges> GetAll()
        {
            //List<RecocileProducer> datas= _context.RecocileProducers.ToList();
            List<ConsultationPledges> datas = new List<ConsultationPledges>();
            ConsultationPledges obj = new ConsultationPledges();
            obj.Base = "11";
            obj.ano = "Lady of the Rosary";
            obj.Entidade_que_paga = "2009";
            obj.Entidade_que_recebe = "648";
            obj.estado = "1";
            obj.nib = "Lady of the Rosary";
            obj.nif = "2009";
            obj.n_entidade = "648";
            obj.tipo_prd = "648";
            obj.valor_a_pagar = "648";
           
            datas.Add(obj);
            ConsultationPledges obj1 = new ConsultationPledges();
            obj1.Base = "12";
            obj1.ano = "eden";
            obj1.Entidade_que_paga = "2019";
            obj1.Entidade_que_recebe = "45";
            obj1.estado = "2";
            obj1.nib = "Lady";
            obj1.nif = "454";
            obj1.n_entidade = "32";
            obj1.tipo_prd = "54654";
            obj1.valor_a_pagar = "324";


            datas.Add(obj1);
            return datas;
        }
    }
}
