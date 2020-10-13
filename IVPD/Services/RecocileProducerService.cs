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
    public interface IRecocileProducerService
    {
        public List<RecocileProducer> GetAll();
    }
    public class RecocileProducerService:IRecocileProducerService
    {
        private VindimaContext _context;

        public RecocileProducerService(VindimaContext context)
        {
            _context = context;
        }
        public List<RecocileProducer> GetAll()
        {
          
            return _context.RecocileProducer.ToList();

            //List<RecocileProducer> datas= _context.RecocileProducers.ToList();
         /*   List<RecocileProducer> datas = new List<RecocileProducer>();
            RecocileProducer obj = new RecocileProducer();
            obj.ano = 1;
            obj.c_Ficheiro_RV = "Lady of the Rosary";
            obj.devolvidos = "2009";
            obj.Naoassinados = "648";
            obj.pendentes = "DL 504-I";
            obj.realizados = "2021";
            obj.retidos = "Replanting right with prior start";
            obj.saldos = "648";
            obj.s_Ficheiro_RV = "2008";
            obj.Transferido_CP = "1025478";
            obj.apurado = 15;
            obj.extrato = 17;
            datas.Add(obj);
            RecocileProducer obj1 = new RecocileProducer();
            obj1.ano = 2;
            obj1.c_Ficheiro_RV = "Lady of the Rosary 12354";
            obj1.devolvidos = "2019";
            obj1.Naoassinados = "6480";
            obj1.pendentes = "FL 504-I";
            obj1.realizados = "2025";
            obj1.retidos = "Replanting right with prior start 874521";
            obj1.saldos = "6498";
            obj1.s_Ficheiro_RV = "2018";
            obj1.Transferido_CP = "1025477418";
            obj1.apurado = 12;
            obj1.extrato = 13;
            datas.Add(obj1);
            return datas;*/
        }
    }
}
