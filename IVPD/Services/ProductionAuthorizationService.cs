using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IProductionAuthorizationService
    {
        public List<ProductionAuthorization> GetAll();
    }
    public class ProductionAuthorizationService : IProductionAuthorizationService
    {
        private VindimaContext _context;

        public ProductionAuthorizationService(VindimaContext context)
        {
            _context = context;
        }
        public List<ProductionAuthorization> GetAll()
        {
            return _context.ProductionAuthorization.ToList();

        }

    }
}
