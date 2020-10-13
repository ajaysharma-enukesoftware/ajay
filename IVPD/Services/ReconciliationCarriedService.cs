using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IReconciliationCarriedService
    {
        public List<ReconciliationCarried> GetAll();
    }
    public class ReconciliationCarriedService : IReconciliationCarriedService
    {
        private IVPDContext _context;

        public ReconciliationCarriedService(IVPDContext context)
        {
            _context = context;
        }
        public List<ReconciliationCarried> GetAll()
        {
            //List<RecocileProducer> datas= _context.RecocileProducers.ToList();
            List<ReconciliationCarried> datas = new List<ReconciliationCarried>();
            ReconciliationCarried obj = new ReconciliationCarried();
            obj.data = "1";
            obj.C_Ficheiros = "Lady of the Rosary";
            obj.não_assinados = "2009";
            obj.observações = "648";
            obj.PagDevolvidos = "DL 504-I";
            obj.PagRealizados = "2021";
            obj.Pendentes = "Replanting right with prior start";
            obj.SaldoApurado = "648";
            obj.SaldoExtrato = "2008";
            obj.S_Ficheiro = "1025478";
            obj.Transf_p_CP = "1025478";
            obj.utilizador = "1025478";
            obj.Retidos = "dsds";

            datas.Add(obj);
            ReconciliationCarried obj1 = new ReconciliationCarried();
            obj1.data = "2";
            obj1.C_Ficheiros = "Lady of the Rosary";
            obj1.não_assinados = "2009";
            obj1.observações = "648";
            obj1.PagDevolvidos = "DL 504-I";
            obj1.PagRealizados = "2021";
            obj1.Pendentes = "Replanting right with prior start";
            obj1.SaldoApurado = "648";
            obj1.SaldoExtrato = "2008";
            obj1.S_Ficheiro = "1025478";
            obj1.Transf_p_CP = "1025478";
            obj1.utilizador = "1025478";
            obj1.Retidos = "dsds";

            datas.Add(obj1);
            return datas;
        }
    }
}
