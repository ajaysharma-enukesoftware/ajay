using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IFileDetailService
    {
        public List<FileDetail> GetAll();
    }
    public class FileDetailService: IFileDetailService
    {
        private IVPDContext _context;

        public FileDetailService(IVPDContext context)
        {
            _context = context;
        }
        public List<FileDetail> GetAll()
        {
            //List<RecocileProducer> datas= _context.RecocileProducers.ToList();
            List<FileDetail> datas = new List<FileDetail>();
            FileDetail obj = new FileDetail();
            obj.estado = "11";
            obj.nib = "Lady of the Rosary";
            obj.nome = "2009";
            obj.n_carta = "648";
            obj.n_vit = "1";
            obj.valor_a_pagar = "Lady of the Rosary";
          
            datas.Add(obj);
            FileDetail obj1 = new FileDetail();
            obj1.estado = "12";
            obj1.nib = "vipul";
            obj1.nome = "20019";
            obj1.n_carta = "2";
            obj1.n_vit = "2";
            obj1.valor_a_pagar = "Lady of the vipul";

           

            datas.Add(obj1);
            return datas;
        }

    }
}
