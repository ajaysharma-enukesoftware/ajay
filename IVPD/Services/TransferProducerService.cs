
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
    public interface ITransferProducerService
    {
        public List<TransferProducer> GetAll();
    }
    public class TransferProducerService:ITransferProducerService
    {
        private IVPDContext _context;

        public TransferProducerService(IVPDContext context)
        {
            _context = context;
        }
        public List<TransferProducer> GetAll()
        {
            //List<TransferProducer> datas= _context.TransferProducers.ToList();
            List<TransferProducer> datas = new List<TransferProducer>();
            TransferProducer obj = new TransferProducer();
            obj.data = "2021";
            obj.entidade = "2008";
            obj.Mov_Orig = "1025478";
            obj.n = 1;
            obj.n_ent = "Lady of the Rosary";
            obj.observacoes = "2021";
            obj.tipo = "Replanting right with prior start";
            obj.valor = "2009";
            datas.Add(obj);
            TransferProducer obj1 = new TransferProducer();
            obj1.data = "2025";
            obj1.entidade = "2018";
            obj1.Mov_Orig = "1027415478";
            obj1.n = 2;
            obj1.n_ent = "Lady of the Rosary 87452";
            obj1.observacoes = "2021";
            obj1.tipo = "Replanting right with prior start 741";
            obj1.valor = "2019";
            datas.Add(obj1);
            return datas;
        }
    }
}
