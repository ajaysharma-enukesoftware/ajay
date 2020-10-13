using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface ILQBaseService
    {
        public List<LQBase> GetAll();
    }
    public class LQBaseService: ILQBaseService
    {
        private IVPDContext _context;

        public LQBaseService(IVPDContext context)
        {
            _context = context;
        }
        public List<LQBase> GetAll()
        {
            //List<RecocileProducer> datas= _context.RecocileProducers.ToList();
            List<LQBase> datas = new List<LQBase>();
            LQBase obj = new LQBase();
            obj.diferenca = "1";
            obj.mg_dcp = "test";
            obj.mg_rv = "2009";
            obj.nome_comprador = "648";
            obj.nome_vendedor = "DL 504-I";
            obj.n_vit = "2021";
          

            datas.Add(obj);
            LQBase obj1 = new LQBase();
            obj1.diferenca = "2";
            obj1.mg_dcp = "testing";
            obj1.mg_rv = "2009";
            obj1.nome_comprador = "648";
            obj1.nome_vendedor = "DL 504-I";
            obj1.n_vit = "2021";

            datas.Add(obj1);
            return datas;
        }
    }
}
