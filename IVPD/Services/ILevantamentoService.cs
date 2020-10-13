using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Models;
using IVPD.Helpers;
using Microsoft.EntityFrameworkCore;
using Audit.Core;
using System.Linq.Dynamic.Core;

namespace IVPD.Services
{
    public interface ILevantamentoService
    {
        public List<Levantamento> GetAll(FilterClass fc, out Pagination p);
    }
    public class LevantamentoService : ILevantamentoService
    {
        private IVPDContext _context;

        public LevantamentoService(IVPDContext context)
        {
            _context = context;
        }
        public List<Levantamento> GetAll(FilterClass fc, out Pagination p)
        {
            string nif = null;
            string nrvitic = null;
            object check;
            if (fc.Filters != null)
            {
                nif = (fc.Filters.TryGetValue("nif", out check) ? Convert.ToString(fc.Filters["nif"]) : null);
                nrvitic = (fc.Filters.TryGetValue("nrvitic", out check) ? Convert.ToString(fc.Filters["nrvitic"]) : null);
            }

            List<Levantamento> Levantamentos = _context.LEVANTAMENTO.ToList().Where(w => !string.IsNullOrEmpty(nif) ? Convert.ToString(w.NIF).Contains(nif) : true).
                Where(w => !string.IsNullOrEmpty(nrvitic) ? Convert.ToString(w.NRVITIC).Contains(nrvitic) : true).ToList();

            string sortType = "ASC";
            if ((bool)fc.IsSortTypeDESC)
            {
                sortType = "DESC";
            }

            if (!string.IsNullOrEmpty(fc.SortBy))
            {
                var propertyInfo = typeof(Levantamento).GetProperty(fc.SortBy.ToUpper());
                if (propertyInfo != null)
                {
                    Levantamentos = Levantamentos.AsQueryable().OrderBy($"{fc.SortBy.ToUpper()} {sortType}").ToList();
                }
            }
            else
            {
                fc.SortBy = "ID";
                Levantamentos = Levantamentos.AsQueryable().OrderBy($"{fc.SortBy.ToUpper()} {sortType}").ToList();
            }

            p = new Pagination();
            p.CurrentPage = (int)fc.Page;
            p.Limit = (int)fc.PageSize;
            p.Total = Levantamentos.Count();

            if ((bool)fc.IsPagination)
            {
                if (fc.Page == null)
                {
                    fc.Page = 1;
                }
                if (fc.PageSize == null)
                {
                    fc.PageSize = 10;
                }

                Levantamentos = Levantamentos.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
            }
            return Levantamentos;
        }
    }
}
