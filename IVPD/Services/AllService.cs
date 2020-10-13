using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVPD.Models;
using IVPD.Helpers;
using Microsoft.EntityFrameworkCore;
using Audit.Core;
using Newtonsoft.Json;

namespace IVPD.Services
{

    public interface IAllService
    {
        public List<Country> CountryGetAll();

        public Country CountryGetById(long id);

        public List<District> DistrictGetAll();

        public District DistictGetById(long id);

        public List<Parish> ParishGetAll();

        public List<ClassPlantation> ClassPlantationGetAll();
        public List<SitucaoDaParcela> SitucaoDaParcelaGetAll();

        public Parish ParishGetById(long id);



        public List<PlotSituation> PlotSituationGetAll();

        public PlotSituation PlotSituationGetById(long id);

        public LegalSituation LegalSituationGetById(long id);

        public List<LegalSituation> LegalSituationGetAll();

        public List<LitigationSituation> LitigationSituationGetAll();

        public List<ExplorerType> ExplorerTypeGetAll();

        public List<DouroPort> DouroPortGetAll();


        public List<colors> colorsGetAll();
        public List<Synonyms> SynonymsTypeGetAll();
    }
    public class AllService:IAllService
    {

        private IVPDContext _context;

        public AllService(IVPDContext context)
        {
            _context = context;
        }

        public List<Country> CountryGetAll()
        {
           return  _context.Country.ToList();
        }

        public Country CountryGetById(long id)
        {
            Country u = _context.Country.Where(w => w.Id == id).ToList().FirstOrDefault();
            return u;
        }

        public List<District> DistrictGetAll()
        {
            return _context.District.ToList();
        }

        public District DistictGetById(long id)
        {
            District u = _context.District.Where(w => w.Id == id).ToList().FirstOrDefault();
            return u;
        }


        public List<Parish> ParishGetAll()
        {
            return _context.Parish .ToList();
        }
        public List<SitucaoDaParcela> SitucaoDaParcelaGetAll()
        {
            return _context.SitucaoDaParcela.ToList();
        }

        
        public Parish ParishGetById(long id)
        {
            Parish u = _context.Parish.Where(w => w.Id == id).ToList().FirstOrDefault();
            return u;
        }


        public List<LegalSituation> LegalSituationGetAll()
        {
            return _context.SITLEGAL.ToList();
        }

        public LegalSituation LegalSituationGetById(long id)
        {
            LegalSituation u = _context.SITLEGAL.Where(w => w.Idsitleg == id).ToList().FirstOrDefault();
            return u;
        }


        public List<PlotSituation> PlotSituationGetAll()
        {
            List<PlotSituation> plotSituations= _context.PlotSituation.ToList();
            return plotSituations;
        }

        public List<ClassPlantation> ClassPlantationGetAll()
        {
            return _context.ClassePlant.ToList();
        }

        public List<LitigationSituation> LitigationSituationGetAll()
        {
            return _context.SITLITIGIO.ToList();
        }

        public PlotSituation PlotSituationGetById(long id)
        {
            PlotSituation u = _context.PlotSituation.Where(w => w.Id == id).ToList().FirstOrDefault();
            return u;
        }

        public List<ExplorerType> ExplorerTypeGetAll()
        {

            return _context.TIPOEXPLOR.ToList(); 
        }

        public List<DouroPort> DouroPortGetAll()
        {
            return _context.DouroPort.ToList();
        }

        public List<colors> colorsGetAll()
        {
            return _context.Cor.ToList();
        }

        public List<Synonyms> SynonymsTypeGetAll()
        {
            return _context.Synonyms.ToList();
        }


    }
}
