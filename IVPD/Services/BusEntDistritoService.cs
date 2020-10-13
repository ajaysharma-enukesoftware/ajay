using IVPD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IBusEntDistritoService
    {
        public List<BusEntDistrito> BusEntDistritoGetAll();

    }
    public class BusEntDistritoService : IBusEntDistritoService
    {
        private IVPDContext _context;


        public BusEntDistritoService(IVPDContext context)
        {
            _context = context;
        }


        public List<BusEntDistrito> BusEntDistritoGetAll()
        {

            return _context.Distrito.OrderBy(o => o.Desdis).ToList();
        }
        
    }
  

}
