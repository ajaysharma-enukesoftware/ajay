
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Helpers;
using IVPD.Models;
using Newtonsoft.Json;

namespace IVPD.Services
{
    public interface IRulesService
    {
        List<Rules> GetAllParcels();

    }
    public class RulesService : IRulesService
    {
        private IVPDContext _context;

        public RulesService(IVPDContext context)
        {
            _context = context;
        }
        public List<Rules> GetAllParcels()
        {
            return _context.Rules.Where(w => w.ModuleParcel == true).ToList();
        }
    }

}
