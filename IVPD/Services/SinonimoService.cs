using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Models;
using IVPD.Helpers;
using Microsoft.EntityFrameworkCore;
using Audit.Core;
using System;
using Newtonsoft.Json;

namespace IVPD.Services
{

    public interface ISinonimoService
    {
        public List<SINONIMO> GetAll();
    }

    public class SinonimoService : ISinonimoService
    {
        private IVPDContext _context;

        public SinonimoService(IVPDContext context)
        {
            _context = context;
        }
        public List<SINONIMO> GetAll()
        {
            List<SINONIMO> data = _context.SINONIMO.ToList();
            return data;
        }

    }
}
