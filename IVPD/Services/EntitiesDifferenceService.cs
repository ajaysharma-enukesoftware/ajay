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
    public interface IEntitiesDifferenceService
    {
        public List<EntitiesDifference> GetAll();
    }
    public class EntitiesDifferenceService:IEntitiesDifferenceService
    {
        private IVPDContext _context;

        public EntitiesDifferenceService(IVPDContext context)
        {
            _context = context;
        }
        public List<EntitiesDifference> GetAll()
        {
            //List<EntitiesDifference> datas= _context.EntitiesDifferences.ToList();
            List<EntitiesDifference> datas = new List<EntitiesDifference>();
            EntitiesDifference obj = new EntitiesDifference();
            obj.Diferenca = "2021";
            obj.entidade = "2008";
            obj.FicheirosRV = "1025478";
            obj.n_entidade = "Testing";
            obj.transferenciaCP = "Lady of the Rosary";
            datas.Add(obj);
            EntitiesDifference obj1 = new EntitiesDifference();
            obj1.Diferenca = "2011";
            obj1.entidade = "2018";
            obj1.FicheirosRV = "15478";
            obj1.n_entidade = "Testing 123";
            obj1.transferenciaCP = "Lady of the Rosary 123";
            datas.Add(obj1);
            return datas;
        }
    }
}
