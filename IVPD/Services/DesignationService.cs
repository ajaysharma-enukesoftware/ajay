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
    public interface IDesignationService
    {
        public List<Designation> GetAll();
    }
    public class DesignationService:IDesignationService
    {
        private IVPDContext _context;

        public DesignationService(IVPDContext context)
        {
            _context = context;
        }
        public List<Designation> GetAll()
        {
            List<Designation> designations= _context.Designations.ToList();
            return designations;
        }
    }
}
