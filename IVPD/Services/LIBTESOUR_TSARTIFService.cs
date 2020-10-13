using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface ILIBTESOUR_TSARTIFService
    {
        public List<LIBTESOUR_TSARTIF> LIBTESOUR_TSARTIFGetAll();

    }

    public class LIBTESOUR_TSARTIFService: ILIBTESOUR_TSARTIFService
    {
        private RevenueContext _context;

        public LIBTESOUR_TSARTIFService(RevenueContext context)
        {
            _context = context;
        }

        public List<LIBTESOUR_TSARTIF> LIBTESOUR_TSARTIFGetAll()
        {
            return _context.LIBTESOUR_TSARTIF.ToList();
        }

    }
}
