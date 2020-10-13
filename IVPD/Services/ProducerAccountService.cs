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
    public interface IProducerAccountService
    {
        public List<ProducerAccount> GetAll();
    }
    public class ProducerAccountService:IProducerAccountService
    {
        private IVPDContext _context;

        public ProducerAccountService(IVPDContext context)
        {
            _context = context;
        }
        public List<ProducerAccount> GetAll()
        {
            //List<ProducerAccount> datas= _context.ProducerAccounts.ToList();
            List<ProducerAccount> datas = new List<ProducerAccount>();
            ProducerAccount obj = new ProducerAccount();
            obj.Conciliacoes_Realizadas = "741";
            obj.de_transferencias_relizadas = "852";
            obj.Nao_assinados = "7458";
            obj.n_cartas_CartasA = "9685";
            obj.n_cartas_CartasB = "456";
            obj.n_cartas_Contratos = "123";
            obj.Pagamentos_devolvidos = "456";
            obj.Pagamentos_pendentes = "789";
            obj.Pagamentos_realizados = "951";
            obj.Pagamentos_retidos = "753";
            obj.Saldo_Apurado = "741";
            obj.sem_ficheiros_RV = "852";
            obj.valor_CartasA = "963";
            obj.valor_CartasB = "874";
            obj.valor_Contratos = "965";
            datas.Add(obj);
            ProducerAccount obj1 = new ProducerAccount();
            obj1.Conciliacoes_Realizadas = "7412";
            obj1.de_transferencias_relizadas = "8512";
            obj1.Nao_assinados = "71458";
            obj1.n_cartas_CartasA = "9285";
            obj1.n_cartas_CartasB = "4156";
            obj1.n_cartas_Contratos = "4123";
            obj1.Pagamentos_devolvidos = "4536";
            obj1.Pagamentos_pendentes = "7879";
            obj1.Pagamentos_realizados = "9951";
            obj1.Pagamentos_retidos = "7553";
            obj1.Saldo_Apurado = "7241";
            obj1.sem_ficheiros_RV = "8521";
            obj1.valor_CartasA = "9623";
            obj1.valor_CartasB = "8174";
            obj1.valor_Contratos = "9165";
            datas.Add(obj1);
            return datas;
        }
    }
}
