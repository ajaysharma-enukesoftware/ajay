using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Models;
using IVPD.Helpers;
using Microsoft.EntityFrameworkCore;
using Audit.Core;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.EntityFrameworkCore.Internal;

namespace IVPD.Services
{
    public interface IBusEntidadeEstatutoService
    {
        public List<BusEntidadeEstatuto> GetAll();
        public List<BusEntidadeEstatutoList> GetAllName();
        public APIResponse Create(BusEntidadeEstatutoList obj);
        public APIResponse Update(BusEntidadeEstatutoList obj);
        public APIResponse Delete(int i);
        public BusEntidadeEstatutoList GetByID(int i);
        public List<BusEntidadeEstatuto> CheckPermission(int entid);
    }
    public class BusEntidadeEstatutoService : IBusEntidadeEstatutoService
    {
        private IVPDSBaseContext _context;
        private IVPDContext _context2;

        public BusEntidadeEstatutoService(IVPDSBaseContext context, IVPDContext context2)
        {
            _context = context;
            _context2 = context2;
        }
        public List<BusEntidadeEstatuto> GetAll()
        {
            List<BusEntidadeEstatuto> alldata = _context.BusEntidadeEstatuto.ToList();
            return alldata;
        }

        public List<BusEntidadeEstatutoList> GetAllName()
        {
            List<BusEntidadeEstatuto> alldata = _context.BusEntidadeEstatuto.ToList();
            List<int> check = _context.BusEntidadeEstatuto.GroupBy(i => new { i.codEntidade })
                                            .Select(g => Convert.ToInt32(g.Key.codEntidade)).ToList();
            List<BusEntidadeEstatutoList> alllist = new List<BusEntidadeEstatutoList>();
            foreach (int item in check)
            {
                BusEntidadeEstatutoList l = new BusEntidadeEstatutoList();
                l.codEntidade = item;
                l.codEntidadeName = _context.BusEntidade.Where(w => w.codEntidade == item).Select(s => s.nome).FirstOrDefault();
                int[] arrEstatuto = _context.BusEntidadeEstatuto.Where(w => w.codEntidade == item).Select(s => Convert.ToInt32(s.codEstatuto)).ToArray();
                l.codEstatuto = arrEstatuto;
                string nameEstatuto = "";
                if (arrEstatuto.Count() > 0)
                {
                    foreach (int item1 in arrEstatuto)
                    {
                        string name = _context2.Estatuto.Where(w => w.CodEstatuto == item1).Select(s => s.Description).FirstOrDefault();
                        if (!string.IsNullOrEmpty(name))
                        {
                            nameEstatuto = nameEstatuto + name + " ,";
                        }
                    }
                    if (!string.IsNullOrEmpty(nameEstatuto))
                    {
                        nameEstatuto = nameEstatuto.Substring(0, (nameEstatuto.Length) - 2);
                    }
                }
                l.codEstatutoName = nameEstatuto;
                alllist.Add(l);
            }
            return alllist;
        }


        public List<BusEntidadeEstatuto> CheckPermission(int entid)
        {
            try
            {
                return _context.BusEntidadeEstatuto.Where(w => w.codEntidade == entid).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public APIResponse Create(BusEntidadeEstatutoList obj)
        {
            try
            {
                int[] check = _context.BusEntidadeEstatuto.GroupBy(i => new { i.codEntidade })
                                            .Select(g => Convert.ToInt32(g.Key.codEntidade)).ToArray();
                bool result = check.Any(a => a == obj.codEntidade);
                if (result)
                {
                    return new APIResponse(false, obj.codEntidade, "Entity already exist!");
                }
                else
                {
                    foreach (int item in obj.codEstatuto)
                    {
                        BusEntidadeEstatuto l = new BusEntidadeEstatuto();
                        l.codEntidade = obj.codEntidade;
                        l.codEstatuto = item;
                        _context.BusEntidadeEstatuto.Add(l);
                        _context.SaveChanges();
                    }
                    return new APIResponse(true, obj.codEntidade, "Data saved successfully!");
                }
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", Convert.ToString(ex.Message));
            }
        }

        public APIResponse Update(BusEntidadeEstatutoList obj)
        {
            try
            {
                List<BusEntidadeEstatuto> list = _context.BusEntidadeEstatuto.Where(w => w.codEntidade == obj.codEntidade).Select(s => s).ToList();
                if (list != null)
                {
                    foreach (BusEntidadeEstatuto item1 in list)
                    {
                        _context.BusEntidadeEstatuto.Remove(item1);
                        _context.SaveChanges();
                    }
                }

                foreach (int item in obj.codEstatuto)
                {
                    BusEntidadeEstatuto l = new BusEntidadeEstatuto();
                    l.codEntidade = obj.codEntidade;
                    l.codEstatuto = item;
                    _context.BusEntidadeEstatuto.Add(l);
                    _context.SaveChanges();
                }
                return new APIResponse(true, obj.codEntidade, "Data updated successfully!");

            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", Convert.ToString(ex.Message));
            }
        }

        public APIResponse Delete(int i)
        {
            try
            {
                List<BusEntidadeEstatuto> list = _context.BusEntidadeEstatuto.Where(w => w.codEntidade == i).Select(s => s).ToList();
                if (list != null)
                {
                    foreach (BusEntidadeEstatuto item1 in list)
                    {
                        _context.BusEntidadeEstatuto.Remove(item1);
                        _context.SaveChanges();
                    }
                }
                return new APIResponse(true, "", "Data deleted successfully!");
            }
            catch (Exception ex)
            {
                return new APIResponse(false, "", Convert.ToString(ex.Message));
            }
        }

        public BusEntidadeEstatutoList GetByID(int i)
        {
            BusEntidadeEstatutoList l = new BusEntidadeEstatutoList();
            l.codEntidade = i;
            l.codEntidadeName = _context.BusEntidade.Where(w => w.codEntidade == i).Select(s => s.nome).FirstOrDefault();
            int[] arrEstatuto = _context.BusEntidadeEstatuto.Where(w => w.codEntidade == i).Select(s => Convert.ToInt32(s.codEstatuto)).ToArray();
            l.codEstatuto = arrEstatuto;
            string nameEstatuto = "";
            if (arrEstatuto.Count() > 0)
            {
                foreach (int item1 in arrEstatuto)
                {
                    string name = _context2.Estatuto.Where(w => w.CodEstatuto == item1).Select(s => s.Description).FirstOrDefault();
                    if (!string.IsNullOrEmpty(name))
                    {
                        nameEstatuto = nameEstatuto + name + " ,";
                    }
                }
                if (!string.IsNullOrEmpty(nameEstatuto))
                {
                    nameEstatuto = nameEstatuto.Substring(0, (nameEstatuto.Length) - 2);
                }
            }
            l.codEstatutoName = nameEstatuto;
            return l;
        }
    }
}