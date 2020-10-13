using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IRegisterMovementsService
    {
        public List<RegisterMovements> GetAll();
    }
    public class RegisterMovementsService : IRegisterMovementsService
    {
        private IVPDContext _context;

        public RegisterMovementsService(IVPDContext context)
        {
            _context = context;
        }
        public List<RegisterMovements> GetAll()
        {
            //List<RecocileProducer> datas= _context.RecocileProducers.ToList();
            List<RegisterMovements> datas = new List<RegisterMovements>();
            RegisterMovements obj = new RegisterMovements();
            obj.Base = "1";
            obj.n_viticultor = "2009";
            obj.outros_pagamentos = "648";
            obj.Quantidade = "DL 504-I";
            obj.Tipo_Prd = "2021";
            obj.Valor_a_pagar = "winter is coming";
            obj.valor_pago = "bb";
            obj.valor_pipa = "ss";
            obj.Valor_retido = "upside down";
           

            datas.Add(obj);
            RegisterMovements obj1 = new RegisterMovements();
            obj1.Base = "1";
            obj1.n_viticultor = "2009";
            obj1.outros_pagamentos = "648";
            obj1.Quantidade = "DL 504-I";
            obj1.Tipo_Prd = "2021";
            obj1.Valor_a_pagar = "summer is coming";
            obj1.valor_pago = "bb";
            obj1.valor_pipa = "ss";
            obj1.Valor_retido = "upside up";

            datas.Add(obj1);
            return datas;
        }
    }
}
