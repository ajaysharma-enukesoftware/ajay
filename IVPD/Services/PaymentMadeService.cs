
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
    public interface IPaymentMadeService
    {
        public List<PaymentMade> GetAll();
    }
    public class PaymentMadeService:IPaymentMadeService
    {
        private IVPDContext _context;

        public PaymentMadeService(IVPDContext context)
        {
            _context = context;
        }
        public List<PaymentMade> GetAll()
        {
            //List<PaymentMade> datas= _context.PaymentMades.ToList();
            List<PaymentMade> datas = new List<PaymentMade>();
            PaymentMade obj = new PaymentMade();
            obj.Base = "2021";
            obj.Data_Entrada = "2008";
            obj.Data_transfer = "1025478";
            obj.Ent_que_paga = "1";
            obj.Ent_recebe = "Lady of the Rosary";
            obj.Nif_destino = "2021";
            obj.N_Carta = "Replanting right with prior start";
            obj.N_Ficheiro = 1;
            obj.Observacoes = "2009";
            obj.Outros_pagamentos = "12";
            obj.Preco_pipa = "123";
            obj.Qnt = "2019";
            obj.Tipo_Prd = "Testing";
            obj.Topi_pagamento = "Test";
            obj.Valor_a_pagar = "Check";
            obj.Menor_que = "Checking 741";
            obj.Menor_ou_igual_a = "Checking 123";
            obj.Igual = "Checking 789";
            obj.Entre = "Checking 965";
            obj.Maior_que = "Checking 52";
            obj.Maior_ou_igual_a = "Checking 1";
            obj.Date_picker_1 = "2020-01-01";
            obj.Date_picker_2 = "2020-02-02";
            obj.Transferencia = "Checking 9";
            obj.Entrada = "Checking 1";
            datas.Add(obj);
            PaymentMade obj1 = new PaymentMade();
            obj1.Base = "2022";
            obj1.Data_Entrada = "2018";
            obj1.Data_transfer = "15478";
            obj1.Ent_que_paga = "10";
            obj1.Ent_recebe = "Lady of the Rosary 123";
            obj1.Nif_destino = "2021";
            obj1.N_Carta = "Replanting right with prior start 123";
            obj1.N_Ficheiro = 2;
            obj1.Observacoes = "2019";
            obj1.Outros_pagamentos = "120";
            obj1.Preco_pipa = "1203";
            obj1.Qnt = "2021";
            obj1.Tipo_Prd = "Testing 123";
            obj1.Topi_pagamento = "Test 123";
            obj1.Valor_a_pagar = "Check 123";
            obj1.Valor_ja_Prago = "Checking 123";
            obj1.Menor_que = "Checking 741741 ";
            obj1.Menor_ou_igual_a = "Checking 12123";
            obj1.Igual = "Checking 12789";
            obj1.Entre = "Checking 92265";
            obj1.Maior_que = "Checking 5112";
            obj1.Maior_ou_igual_a = "Checking 123";
            obj1.Date_picker_1 = "2020-03-03";
            obj1.Date_picker_2 = "2020-04-04";
            obj1.Transferencia = "Checking 669";
            obj1.Entrada = "Checking 541";
            datas.Add(obj1);
            return datas;
        }
    }
}
