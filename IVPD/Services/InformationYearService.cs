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
    public interface IInformationYearService
    {
        public List<InformationYear> GetAll();
    }
    public class InformationYearService:IInformationYearService
    {
        private IVPDContext _context;

        public InformationYearService(IVPDContext context)
        {
            _context = context;
        }
        public List<InformationYear> GetAll()
        {
            //List<InformationYear> datas= _context.InformationYears.ToList();
            List<InformationYear> datas = new List<InformationYear>();
            InformationYear obj = new InformationYear();
            obj.year = "2020";
            obj.value = "74125";
            datas.Add(obj);
            InformationYear obj1 = new InformationYear();
            obj1.year = "2019";
            obj1.value = "85225";
            datas.Add(obj1);
            InformationYear obj2 = new InformationYear();
            obj2.year = "2018";
            obj2.value = "85225";
            datas.Add(obj2);
            InformationYear obj3 = new InformationYear();
            obj3.year = "2017";
            obj3.value = "85225";
            datas.Add(obj3);
            InformationYear obj4 = new InformationYear();
            obj4.year = "2016";
            obj4.value = "85225";
            datas.Add(obj4);
            InformationYear obj5 = new InformationYear();
            obj5.year = "2015";
            obj5.value = "85225";
            datas.Add(obj5);
            return datas;
        }
    }
}
