using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using IVPD.Models;
using Microsoft.EntityFrameworkCore;

namespace IVPD.Services
{
    public interface IBusEntidadeService
    {
        IEnumerable<BusEntidade> GetAll(string searchStringNIF);
        IEnumerable<BusEntidade> GetAllFilters(string searchString);

        List<BusEntidade> GetById(int id);

        List<BusEntidade> GetByName(string NIF);

    }
    public class BusEntidadeService : IBusEntidadeService
    {
        private IVPDSBaseContext _context;

        public BusEntidadeService(IVPDSBaseContext context)
        {
            _context = context;
        }
        public IEnumerable<BusEntidade> GetAllFilters(string searchString)
        {
            string query;
              if (searchString.All(char.IsDigit))
              {
            query = "SELECT * FROM BusEntidade WHERE nifap LIKE '"+"%"+searchString+"%'";
              }
              else
              {
            query = "SELECT * FROM BusEntidade WHERE nome LIKE '" + "%" + searchString + "%'";

              }
             

            var EntityList = _context.BusEntidade
                    .FromSqlRaw(query)
                    .ToList();
            
            return EntityList;
        }
        public IEnumerable<BusEntidade> GetAll(string searchStringNIF)
        {
            /*string query = "SELECT * FROM BusEntidade WHERE nif LIKE '" + "%" + searchStringNIF + "%'";
            var EntityList = _context.BusEntidade
                   .FromSqlRaw(query)
                   .ToList();
                   */
            return _context.BusEntidade.AsNoTracking().Where(o => o.nif == searchStringNIF).ToList();
        }
        public List<BusEntidade> GetById(int id)
        {
            List<BusEntidade> entities = _context.BusEntidade.Where(x => x.codEntidade == id).ToList();
            return entities;

        }
        public List<BusEntidade> GetByName(string NIF)
        {
            List<BusEntidade> entities = _context.BusEntidade.Where(x => x.nif == NIF).ToList();
            return entities;

        }
    }

}
