
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
    public interface IEstatutoService
    {
        List<Estatuto> GetAllParcels();
        List<Estatuto> GetAll();
        Estatuto GetById(int id);
        Estatuto Create(Estatuto p);
        void Update(Estatuto p);
        void Delete(int id);
    }
    public class EstatutoService : IEstatutoService
    {
        private IVPDContext _context;

        public EstatutoService(IVPDContext context)
        {
            _context = context;
        }
        public List<Estatuto> GetAllParcels()
        {
            return _context.Estatuto.Where(w => w.ModuleParcel == true).Where(w1 => w1.DeletedAt == null).ToList();
        }

        public List<Estatuto> GetAll()
        {
            return _context.Estatuto.Where(w1 => w1.DeletedAt == null).ToList();
        }

        public Estatuto GetById(int id)
        {
            Estatuto p = _context.Estatuto.AsNoTracking().Where(w => w.CodEstatuto == id).Where(w1 => w1.DeletedAt == null).ToList().FirstOrDefault();
            _context.SaveChanges();
            return p;
        }

        public Estatuto Create(Estatuto obj)
        {
            obj.CreatedAt = DateTime.UtcNow;
            _context.Estatuto.Add(obj);
            _context.SaveChanges();
            obj = _context.Estatuto.ToList().LastOrDefault();
            _context.SaveChanges();
            return obj;
        }

        public void Update(Estatuto p)
        {
            if (p != null)
            {
                p.UpdatedAt = DateTime.UtcNow;
                _context.Estatuto.Update(p);
                _context.SaveChanges();
            }
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var p = _context.Estatuto.Where(w => w.CodEstatuto == id).ToList().FirstOrDefault();
            if (p != null)
            {
                p.DeletedAt = DateTime.UtcNow;
                _context.Estatuto.Update(p);
                _context.SaveChanges();
            }
        }
    }

}
