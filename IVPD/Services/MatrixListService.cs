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

    public interface IMatrixListService
    {
        public List<MatrixList> GetAll();
    }

    public class MatrixListService : IMatrixListService
    {
        private IVPDContext _context;

        public MatrixListService(IVPDContext context)
        {
            _context = context;
        }
        public List<MatrixList> GetAll()
        {
            List<MatrixList> data = _context.MatrixList.ToList();
            return data;
        }
    }
}
