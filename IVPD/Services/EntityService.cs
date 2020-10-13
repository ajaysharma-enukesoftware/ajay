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
    public interface IEntityService
    {
        IEnumerable<Entity> GetAll();
        List<Entity> GetById(int id);

        List<Entity> GetByName(string NIF);

    }
    public class EntityService : IEntityService
    {
        private IVPDContext _context;

        public EntityService(IVPDContext context)
        {
            _context = context;
        }
        public IEnumerable<Entity> GetAll()
        {
            return _context.Entity;
        }
        public List<Entity> GetById(int id)
        {
            List<Entity> entities = _context.Entity.Where(x => x.Id == id).ToList();
            List<Entity> entit = new List<Entity>();
            foreach (Entity entity in entities)
            {
                Entity enti = _context.Entity.Where(x => x.Id == entity.Id).FirstOrDefault();
                if (enti != null)
                {
                    entit.Add(enti);
                }
            }
            return entit;

        }
           public List<Entity> GetByName(string NIF)
        {
            List<Entity> entities = _context.Entity.Where(x => x.NIF == NIF).ToList();
            List<Entity> entit = new List<Entity>();
            foreach (Entity entity in entities)
            {
                Entity enti = _context.Entity.Where(x => x.NIF == entity.NIF).FirstOrDefault();
                if (enti != null)
                {
                    entit.Add(enti);
                }
            }
            return entit;

        }
    }

}
