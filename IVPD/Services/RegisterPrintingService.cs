using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IRegisterPrintingService
    {

        RegisterPrinting Create(RegisterPrinting p);
  
    }
    public class RegisterPrintingService: IRegisterPrintingService
    {
       
        public RegisterPrinting Create(RegisterPrinting obj)
        {
           
            //   _context.RegisterPrinting.Add(obj);
            //   _context.SaveChanges();
          

            return obj;
        }
    }
}
