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
    public interface IPaymentDicoFreService
    {
        public List<PaymentDicoFre> GetAll();
    }
    public class PaymentDicoFreService:IPaymentDicoFreService
    {
        private IVPDContext _context;

        public PaymentDicoFreService(IVPDContext context)
        {
            _context = context;
        }
        public List<PaymentDicoFre> GetAll()
        {
            //List<PaymentDicoFre> datas= _context.PaymentDicoFres.ToList();
            List<PaymentDicoFre> datas = new List<PaymentDicoFre>();
            PaymentDicoFre obj = new PaymentDicoFre();
            obj.entidade = "2021";
            obj.C_Postal = "2008";
            obj.Entidade_Recetora = "1025478";
            obj.Nif_entidade = "Test";
            obj.n_entidade = "Replanting right with prior start";
            obj.nif_pagadora = "8745";
            obj.Valor_recebido = "2009";
            datas.Add(obj);
            PaymentDicoFre obj1 = new PaymentDicoFre();
            obj1.entidade = "2011";
            obj1.C_Postal = "2018";
            obj1.Entidade_Recetora = "1478";
            obj1.Nif_entidade = "Test 123";
            obj1.n_entidade = "Replanting right with prior start 123";
            obj1.nif_pagadora = "87475";
            obj1.Valor_recebido = "2019";
            datas.Add(obj1);
            return datas;
        }
    }
}
