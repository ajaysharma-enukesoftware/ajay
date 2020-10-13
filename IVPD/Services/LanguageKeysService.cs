using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Models;
using IVPD.Helpers;
using Microsoft.EntityFrameworkCore;
using Audit.Core;
using System;

namespace IVPD.Services
{
    public interface ILanguageKeysService
    {
        public List<LanguageKeys> GetAll();

    }
    public class LanguageKeysService : ILanguageKeysService
    {
        private IVPDContext _context;

        public LanguageKeysService(IVPDContext context)
        {
            _context = context;
        }
        public List<LanguageKeys> GetAll()
        {
            return _context.LanguageKeys.ToList();
        }
    }
    }
