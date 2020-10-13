using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface ICastaService
    {
        public List<Casta> CastaGetAll();

    }
    public class CastaService : ICastaService
    {
        private IVPDContext _context;

        public CastaService(IVPDContext context)
        {
            _context = context;
        }

        public List<Casta> CastaGetAll()
        {
            return _context.Casta.OrderBy(o => o.Codcasta).ToList();
        }
    }
}
