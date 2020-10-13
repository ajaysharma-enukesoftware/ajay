using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using IVPD.Helpers;
using IVPD.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace IVPD.Services
{
    public interface IParcelService
    {
        IEnumerable<ParcelList> GetAll(FilterClass fc, out Pagination p);
        Parcel GetById(int id, short versao);
        MainParcelByID GetByAll(int id, short versao);
        Parcel Create(MainParcel p);
        void Update(MainParcel p);
        void Delete(int id, short versao);

        bool CheckID(int id);
    }
    public class ParcelService : IParcelService
    {
        private IVPDContext _context;
        private IVPDSBaseContext _context2;

        public ParcelService(IVPDContext context, IVPDSBaseContext context2)
        {
            _context = context;
            _context2 = context2;
        }


        public bool CheckID(int id)
        {
            try
            {
                Parcel p = _context.Parcela.Where(w => w.NUMPARC == id).FirstOrDefault();
                if (p != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }



        public IEnumerable<ParcelList> GetAll(FilterClass fc, out Pagination p)
        {
            List<ParcelName> parceldata = new List<ParcelName>();

            //Variables defined
            string countryname = null;
            string parishname = null;
            string geocod = null;
            string districtname = null;
            string classe = null;
            string[] countrynamemultiple = null;
            string[] geocodmultiple = null;
            string[] districtnamemultiple = null;
            string[] parishnamemultiple = null;
            string[] classemultiple = null;
            string countrynamelike = null;
            string parishnamelike = null;
            string districtnamelike = null;
            string geocodlike = null;
            string classelike = null;
            string numparclike = null;
            string numparantlike = null;
            string areaaptalike = null;
            string areailegallike = null;
            string areanaptalike = null;
            string areaaflike = null;
            string anoplantacaolike = null;
            string explorerentnum = null;
            string ownerentnum = null;
            string entnum = null;
            int numparc = 0;
            int[] numparcmultiple = null;
            decimal[] areaaptamultiple = null;
            decimal[] areailegalmultiple = null;
            decimal[] areanaptamultiple = null;
            decimal[] areaafmultiple = null;
            decimal[] anoplantacaomultiple = null;
            decimal numparant = 0;
            decimal areaapta = 0;
            decimal areanapta = 0;
            decimal areailegal = 0;
            decimal areaaf = 0;
            decimal anoplantacao = 0;
            bool? sitedeclar = null;
            bool? legalizavel = null;

            //Check Conditions
            object check;
            string checkint = "0";
            if (fc.Filters != null)
            {
                countryname = (fc.Filters.TryGetValue("countryname", out check) ? Convert.ToString(fc.Filters["countryname"]) : null);
                districtname = (fc.Filters.TryGetValue("districtname", out check) ? Convert.ToString(fc.Filters["districtname"]) : null);
                parishname = (fc.Filters.TryGetValue("parishname", out check) ? Convert.ToString(fc.Filters["parishname"]) : null);
                classe = (fc.Filters.TryGetValue("classe", out check) ? Convert.ToString(fc.Filters["classe"]) : null);
                entnum = (fc.Filters.TryGetValue("entnum", out check) ? Convert.ToString(fc.Filters["entnum"]) : null);
                ownerentnum = (fc.Filters.TryGetValue("ownerentnum", out check) ? Convert.ToString(fc.Filters["ownerentnum"]) : null);
                explorerentnum = (fc.Filters.TryGetValue("explorerentnum", out check) ? Convert.ToString(fc.Filters["explorerentnum"]) : null);
                object obj = (fc.Filters.TryGetValue("countrynamemultiple", out check) ? fc.Filters["countrynamemultiple"] : null);
                if (obj != null)
                {
                    obj = obj.ToString().Replace("ValueKind = Array : ", "").Replace("[", "").Replace("]", "");
                    countrynamemultiple = Convert.ToString(obj).Split(',');
                }

                object districtobj = (fc.Filters.TryGetValue("districtnamemultiple", out check) ? fc.Filters["districtnamemultiple"] : null);
                if (districtobj != null)
                {
                    districtobj = districtobj.ToString().Replace("ValueKind = Array : ", "").Replace("[", "").Replace("]", "");
                    districtnamemultiple = Convert.ToString(districtobj).Split(',');
                }

                object parishobj = (fc.Filters.TryGetValue("parishnamemultiple", out check) ? fc.Filters["parishnamemultiple"] : null);
                if (parishobj != null)
                {
                    parishobj = parishobj.ToString().Replace("ValueKind = Array : ", "").Replace("[", "").Replace("]", "");
                    parishnamemultiple = Convert.ToString(parishobj).Split(',');
                }

                object classeobj = (fc.Filters.TryGetValue("classemultiple", out check) ? fc.Filters["classemultiple"] : null);
                if (classeobj != null)
                {
                    classeobj = classeobj.ToString().Replace("ValueKind = Array : ", "").Replace("[", "").Replace("]", "");
                    classemultiple = Convert.ToString(classeobj).Split(',');
                }


                object geoobj = (fc.Filters.TryGetValue("geocodmultiple", out check) ? fc.Filters["geocodmultiple"] : null);
                if (geoobj != null)
                {
                    geoobj = geoobj.ToString().Replace("ValueKind = Array : ", "").Replace("[", "").Replace("]", "");
                    geocodmultiple = Convert.ToString(geoobj).Split(',');
                }

                object obj1 = (fc.Filters.TryGetValue("numparcmultiple", out check) ? fc.Filters["numparcmultiple"] : null);
                if (obj1 != null)
                {
                    obj1 = obj1.ToString().Replace("ValueKind = Array : ", "").Replace("[", "").Replace("]", "");
                    string[] intarr = Convert.ToString(obj1).Split(',');
                    List<int> ii = new List<int>();
                    for (int i = 0; i < intarr.Length; i++)
                    {
                        int val = 0;
                        int.TryParse(intarr[i], out val);
                        ii.Add(val);
                    }
                    numparcmultiple = ii.ToArray();
                }

                object objareaaptamultiple = (fc.Filters.TryGetValue("areaaptamultiple", out check) ? fc.Filters["areaaptamultiple"] : null);
                if (objareaaptamultiple != null)
                {
                    objareaaptamultiple = objareaaptamultiple.ToString().Replace("ValueKind = Array : ", "").Replace("[", "").Replace("]", "");
                    string[] intarr = Convert.ToString(objareaaptamultiple).Split(',');
                    List<decimal> ii = new List<decimal>();
                    for (int i = 0; i < intarr.Length; i++)
                    {
                        decimal val = 0;
                        decimal.TryParse(intarr[i], out val);
                        ii.Add(val);
                    }
                    areaaptamultiple = ii.ToArray();
                }

                object objareaafmultiple = (fc.Filters.TryGetValue("areaafmultiple", out check) ? fc.Filters["areaafmultiple"] : null);
                if (objareaafmultiple != null)
                {
                    objareaafmultiple = objareaafmultiple.ToString().Replace("ValueKind = Array : ", "").Replace("[", "").Replace("]", "");
                    string[] intarr = Convert.ToString(objareaafmultiple).Split(',');
                    List<decimal> ii = new List<decimal>();
                    for (int i = 0; i < intarr.Length; i++)
                    {
                        decimal val = 0;
                        decimal.TryParse(intarr[i], out val);
                        ii.Add(val);
                    }
                    areaafmultiple = ii.ToArray();
                }

                object objareailegalmultiple = (fc.Filters.TryGetValue("areailegalmultiple", out check) ? fc.Filters["areailegalmultiple"] : null);
                if (objareailegalmultiple != null)
                {
                    objareailegalmultiple = objareailegalmultiple.ToString().Replace("ValueKind = Array : ", "").Replace("[", "").Replace("]", "");
                    string[] intarr = Convert.ToString(objareailegalmultiple).Split(',');
                    List<decimal> ii = new List<decimal>();
                    for (int i = 0; i < intarr.Length; i++)
                    {
                        decimal val = 0;
                        decimal.TryParse(intarr[i], out val);
                        ii.Add(val);
                    }
                    areailegalmultiple = ii.ToArray();
                }

                object objanoplantacaomultiple = (fc.Filters.TryGetValue("anoplantacaomultiple", out check) ? fc.Filters["anoplantacaomultiple"] : null);
                if (objanoplantacaomultiple != null)
                {
                    objanoplantacaomultiple = objanoplantacaomultiple.ToString().Replace("ValueKind = Array : ", "").Replace("[", "").Replace("]", "");
                    string[] intarr = Convert.ToString(objanoplantacaomultiple).Split(',');
                    List<decimal> ii = new List<decimal>();
                    for (int i = 0; i < intarr.Length; i++)
                    {
                        decimal val = 0;
                        decimal.TryParse(intarr[i], out val);
                        ii.Add(val);
                    }
                    anoplantacaomultiple = ii.ToArray();
                }


                object objareanaptamultiple = (fc.Filters.TryGetValue("areanaptamultiple", out check) ? fc.Filters["areanaptamultiple"] : null);
                if (objareanaptamultiple != null)
                {
                    objareanaptamultiple = objareanaptamultiple.ToString().Replace("ValueKind = Array : ", "").Replace("[", "").Replace("]", "");
                    string[] intarr = Convert.ToString(objareanaptamultiple).Split(',');
                    List<decimal> ii = new List<decimal>();
                    for (int i = 0; i < intarr.Length; i++)
                    {
                        decimal val = 0;
                        decimal.TryParse(intarr[i], out val);
                        ii.Add(val);
                    }
                    areanaptamultiple = ii.ToArray();
                }

                countrynamelike = (fc.Filters.TryGetValue("countrynamelike", out check) ? Convert.ToString(fc.Filters["countrynamelike"]) : null);
                parishnamelike = (fc.Filters.TryGetValue("parishnamelike", out check) ? Convert.ToString(fc.Filters["parishnamelike"]) : null);
                districtnamelike = (fc.Filters.TryGetValue("districtnamelike", out check) ? Convert.ToString(fc.Filters["districtnamelike"]) : null);
                geocodlike = (fc.Filters.TryGetValue("geocodlike", out check) ? Convert.ToString(fc.Filters["geocodlike"]) : null);
                classelike = (fc.Filters.TryGetValue("classelike", out check) ? Convert.ToString(fc.Filters["classelike"]) : null);
                numparclike = (fc.Filters.TryGetValue("numparclike", out check) ? Convert.ToString(fc.Filters["numparclike"]) : null);
                numparantlike = (fc.Filters.TryGetValue("numparantlike", out check) ? Convert.ToString(fc.Filters["numparantlike"]) : null);
                areaaptalike = (fc.Filters.TryGetValue("areaaptalike", out check) ? Convert.ToString(fc.Filters["areaaptalike"]) : null);
                areailegallike = (fc.Filters.TryGetValue("areailegallike", out check) ? Convert.ToString(fc.Filters["areailegallike"]) : null);
                areanaptalike = (fc.Filters.TryGetValue("areanaptalike", out check) ? Convert.ToString(fc.Filters["areanaptalike"]) : null);
                areaaflike = (fc.Filters.TryGetValue("areaaflike", out check) ? Convert.ToString(fc.Filters["areaaflike"]) : null);
                anoplantacaolike = (fc.Filters.TryGetValue("anoplantacaolike", out check) ? Convert.ToString(fc.Filters["anoplantacaolike"]) : null);

                geocod = (fc.Filters.TryGetValue("geocod", out check) ? Convert.ToString(fc.Filters["geocod"]) : null);
                checkint = (fc.Filters.TryGetValue("numparc", out check) ? Convert.ToString(fc.Filters["numparc"]) : null);
                int.TryParse(checkint, out numparc);

                checkint = (fc.Filters.TryGetValue("numparant", out check) ? Convert.ToString(fc.Filters["numparant"]) : null);
                decimal.TryParse(checkint, out numparant);

                checkint = (fc.Filters.TryGetValue("areaapta", out check) ? Convert.ToString(fc.Filters["areaapta"]) : null);
                decimal.TryParse(checkint, out areaapta);

                checkint = (fc.Filters.TryGetValue("areanapta", out check) ? Convert.ToString(fc.Filters["areanapta"]) : null);
                decimal.TryParse(checkint, out areanapta);

                checkint = (fc.Filters.TryGetValue("areailegal", out check) ? Convert.ToString(fc.Filters["areailegal"]) : null);
                decimal.TryParse(checkint, out areailegal);

                checkint = (fc.Filters.TryGetValue("areaaf", out check) ? Convert.ToString(fc.Filters["areaaf"]) : null);
                decimal.TryParse(checkint, out areaaf);

                checkint = (fc.Filters.TryGetValue("anoplantacao", out check) ? Convert.ToString(fc.Filters["anoplantacao"]) : null);
                decimal.TryParse(checkint, out anoplantacao);

                checkint = (fc.Filters.TryGetValue("sitedeclar", out check) ? Convert.ToString(fc.Filters["sitedeclar"]) : null);
                if (!string.IsNullOrEmpty(checkint))
                {
                    bool b = false;
                    bool.TryParse(checkint, out b);
                    sitedeclar = b;
                }

                checkint = (fc.Filters.TryGetValue("legalizavel", out check) ? Convert.ToString(fc.Filters["legalizavel"]) : null);
                if (!string.IsNullOrEmpty(checkint))
                {
                    bool b = false;
                    bool.TryParse(checkint, out b);
                    legalizavel = b;
                }
            }

            var data = (from ep in _context.Parcela
                        join e in _context.Distrito on ep.CODDIS equals e.Coddis
                        join t in _context.CONCELHO on new { key1 = ep.CODCON, key2 = ep.CODDIS } equals new { key1 = t.Codcon, key2 = t.Coddis }
                        join f in _context.Freguesia on new { key1 = ep.CODCON, key2 = ep.CODDIS, key3 = ep.CODFRG } equals new { key1 = f.CODCON, key2 = f.CODDIS, key3 = f.CODFRG }
                        join exp in _context.EXPLORPARC on new { key1 = ep.NUMPARC, key2 = ep.VERSAO } equals new { key1 = exp.NUMPARC, key2 = exp.VERSAO }
                        join pro in _context.PROPPARC on new { key1 = ep.NUMPARC, key2 = ep.VERSAO } equals new { key1 = pro.NUMPARC, key2 = pro.VERSAO }
                        where
                        !string.IsNullOrEmpty(countryname) ? t.Descon == countryname : true
                        && !string.IsNullOrEmpty(classe) ? ep.CLASSE == classe : true
                        && !string.IsNullOrEmpty(parishname) ? f.DESFRG == parishname : true
                        && !string.IsNullOrEmpty(districtname) ? e.Desdis == districtname : true
                        && (!string.IsNullOrEmpty(countrynamelike) ? t.Descon.Contains(countrynamelike) : true)
                        && (!string.IsNullOrEmpty(parishnamelike) ? f.DESFRG.Contains(parishnamelike) : true)
                        && (!string.IsNullOrEmpty(districtnamelike) ? e.Desdis.Contains(districtnamelike) : true)
                        && (!string.IsNullOrEmpty(numparclike) ? Convert.ToString(ep.NUMPARC).Contains(numparclike) : true)
                        && (!string.IsNullOrEmpty(numparantlike) ? Convert.ToString(ep.NUMPARANT).Contains(numparantlike) : true)
                        && (!string.IsNullOrEmpty(anoplantacaolike) ? Convert.ToString(ep.ANOPLANTACAO).Contains(anoplantacaolike) : true)
                        && (!string.IsNullOrEmpty(geocodlike) ? ep.GEOCOD.Contains(geocodlike) : true)
                        && (!string.IsNullOrEmpty(geocod) ? ep.GEOCOD == geocod : true)
                        && (!string.IsNullOrEmpty(explorerentnum) ? exp.ENTNUM.Trim() == explorerentnum.Trim() : true)
                        && (!string.IsNullOrEmpty(ownerentnum) ? pro.ENTNUM.Trim() == ownerentnum.Trim() : true)
                        && (numparc > 0 ? ep.NUMPARC == numparc : true)
                        && (anoplantacao > 0 ? ep.ANOPLANTACAO == anoplantacao : true)
                        && (numparant > 0 ? ep.NUMPARANT == numparant : true)
                        && (areaapta > 0 ? exp.AREAAPTA == areaapta : true)
                        && (areailegal > 0 ? exp.AREAILEGAL == areailegal : true)
                        && (areanapta > 0 ? exp.AREANAPTA == areanapta : true)
                        && (areaaf > 0 ? exp.AREAAF == areaaf : true)
                        && (sitedeclar != null ? ep.SITDECLAR == sitedeclar : true)
                        && (legalizavel != null ? ep.LEGALIZAVEL == legalizavel : true)
                        && (!string.IsNullOrEmpty(areaaptalike) ? Convert.ToString(exp.AREANAPTA).Contains(areaaptalike) : true)
                        && (!string.IsNullOrEmpty(areailegallike) ? Convert.ToString(exp.AREAILEGAL).Contains(areailegallike) : true)
                        && (!string.IsNullOrEmpty(areanaptalike) ? Convert.ToString(exp.AREANAPTA).Contains(areanaptalike) : true)
                        && (!string.IsNullOrEmpty(areaaflike) ? Convert.ToString(exp.AREAAF).Contains(areaaflike) : true)
                        && (numparcmultiple != null ? numparcmultiple.Contains(ep.NUMPARC) : true)
                        && (areaaptamultiple != null ? areaaptamultiple.Contains((decimal)exp.AREAAPTA) : true)
                        && (areailegalmultiple != null ? areailegalmultiple.Contains((decimal)exp.AREAILEGAL) : true)
                        && (areanaptamultiple != null ? areanaptamultiple.Contains((decimal)exp.AREANAPTA) : true)
                        && (geocodmultiple != null ? !string.IsNullOrEmpty(ep.GEOCOD) : true)
                        && ep.STATUS == "A"
                        && (!string.IsNullOrEmpty(entnum) ? ((pro.ENTNUM.Trim() == entnum.Trim()) || (exp.ENTNUM.Trim() == entnum.Trim())) : true)
                        select new ParcelName()
                        {

                            CODCON = ep.CODCON,
                            CODFRG = ep.CODFRG,
                            CODDIS = ep.CODDIS,
                            CODLITIGIO = ep.CODLITIGIO,
                            CountryName = t.Descon,
                            DistrictName = e.Desdis,
                            IDSITLEG = ep.IDSITLEG,
                            NUMPARC = ep.NUMPARC,
                            ParishName = f.DESFRG,
                            VERSAO = ep.VERSAO,
                            ANOPLANTACAO = ep.ANOPLANTACAO,
                            DESPARC = ep.DESPARC,
                            DTULTVIST = ep.DTULTVIST,
                            GEOCOD = ep.GEOCOD,
                            IDSITPARC = ep.IDSITPARC,
                            IDTPESTADOVINHA = ep.IDTPESTADOVINHA,
                            LEGALIZAVEL = ep.LEGALIZAVEL,
                            NUMPARANT = ep.NUMPARANT,
                            PONTUACAO = ep.PONTUACAO,
                            SITEDECLAR = ep.SITDECLAR,
                            CLASSE = ep.CLASSE

                        }).Distinct().ToList();

            parceldata = data.Where(x => ((countrynamemultiple != null) ? countrynamemultiple.ToList().Any(s => s.Contains(x.CountryName)) : true)).
                Where(x => ((anoplantacaomultiple != null) ? anoplantacaomultiple.Contains((decimal)x.ANOPLANTACAO) : true))
                .Where(x => ((geocodmultiple != null) ? geocodmultiple.ToList().Any(s => s.Contains(x.GEOCOD)) : true)).
                Where(x => ((districtnamemultiple != null) ? districtnamemultiple.ToList().Any(s => s.Contains(x.DistrictName)) : true)).
                Where(x => ((classemultiple != null) ? classemultiple.ToList().Any(s => s.Contains(x.CLASSE)) : true)).
                 Where(x => ((parishnamemultiple != null) ? parishnamemultiple.ToList().Any(s => s.Contains(x.ParishName)) : true))
                 .ToList();

            string sortType = "ASC";
            if ((bool)fc.IsSortTypeDESC)
            {
                sortType = "DESC";
            }

            p = new Pagination();
            p.CurrentPage = (int)fc.Page;
            p.Limit = (int)fc.PageSize;
            p.Total = parceldata.Count();

            if (!string.IsNullOrEmpty(fc.SortBy))
            {
                if (fc.SortBy.ToLower() == "districtname")
                {
                    fc.SortBy = "DistrictName";
                }
                else if (fc.SortBy.ToLower() == "countryname")
                {
                    fc.SortBy = "CountryName";
                }
                else if (fc.SortBy.ToLower() == "parishname")
                {
                    fc.SortBy = "ParishName";
                }
                else
                {
                    fc.SortBy = fc.SortBy.ToUpper();
                }
                var propertyInfo = typeof(ParcelName).GetProperty(fc.SortBy);
                if (propertyInfo != null)
                {
                    parceldata = parceldata.AsQueryable().OrderBy($"{fc.SortBy} {sortType}").ToList();
                }
            }

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

                parceldata = parceldata.Skip(((int)fc.Page - 1) * (int)fc.PageSize).Take((int)fc.PageSize).ToList();
            }

            List<ParcelList> listdata = new List<ParcelList>();
            foreach (ParcelName item in parceldata)
            {
                ParcelList pl = new ParcelList();
                pl.parcel = item;
                pl.parcelExplorer = _context.EXPLORPARC.Where(w => w.NUMPARC == item.NUMPARC).Where(w => w.VERSAO == item.VERSAO).Where(w => w.STATUS == "A").ToList().ToArray();

                if (!string.IsNullOrEmpty(explorerentnum) && pl.parcelExplorer.Length > 0)
                {
                    pl.parcelExplorer = pl.parcelExplorer.Where(w => w.ENTNUM.Trim() == explorerentnum.Trim()).ToList().ToArray();
                }

                if (areaapta > 0 && pl.parcelExplorer.Length > 0)
                {
                    pl.parcelExplorer = pl.parcelExplorer.Where(w => w.AREAAPTA == areaapta).ToList().ToArray();
                }

                if (areailegal > 0 && pl.parcelExplorer.Length > 0)
                {
                    pl.parcelExplorer = pl.parcelExplorer.Where(w => w.AREAILEGAL == areailegal).ToList().ToArray();
                }

                if (areanapta > 0 && pl.parcelExplorer.Length > 0)
                {
                    pl.parcelExplorer = pl.parcelExplorer.Where(w => w.AREANAPTA == areanapta).ToList().ToArray();
                }

                if (areaaf > 0 && pl.parcelExplorer.Length > 0)
                {
                    pl.parcelExplorer = pl.parcelExplorer.Where(w => w.AREAAF == areaaf).ToList().ToArray();
                }

                if (!string.IsNullOrEmpty(areaaptalike) && pl.parcelExplorer.Length > 0)
                {
                    pl.parcelExplorer = pl.parcelExplorer.Where(w => Convert.ToString(w.AREANAPTA).Contains(areaaptalike)).ToList().ToArray();
                }

                if (!string.IsNullOrEmpty(areailegallike) && pl.parcelExplorer.Length > 0)
                {
                    pl.parcelExplorer = pl.parcelExplorer.Where(w => Convert.ToString(w.AREAILEGAL).Contains(areailegallike)).ToList().ToArray();
                }

                if (!string.IsNullOrEmpty(areanaptalike) && pl.parcelExplorer.Length > 0)
                {
                    pl.parcelExplorer = pl.parcelExplorer.Where(w => Convert.ToString(w.AREANAPTA).Contains(areanaptalike)).ToList().ToArray();
                }

                if (!string.IsNullOrEmpty(areaaflike) && pl.parcelExplorer.Length > 0)
                {
                    pl.parcelExplorer = pl.parcelExplorer.Where(w => Convert.ToString(w.AREAAF).Contains(areaaflike)).ToList().ToArray();
                }

                if (areaaptamultiple != null && pl.parcelExplorer.Length > 0)
                {
                    pl.parcelExplorer = pl.parcelExplorer.Where(w => areaaptamultiple.Contains((decimal)w.AREAAPTA)).ToList().ToArray();
                }

                if (areailegalmultiple != null && pl.parcelExplorer.Length > 0)
                {
                    pl.parcelExplorer = pl.parcelExplorer.Where(w => areailegalmultiple.Contains((decimal)w.AREAILEGAL)).ToList().ToArray();
                }

                if (areanaptamultiple != null && pl.parcelExplorer.Length > 0)
                {
                    pl.parcelExplorer = pl.parcelExplorer.Where(w => areanaptamultiple.Contains((decimal)w.AREANAPTA)).ToList().ToArray();
                }


                pl.parcelProperty = _context.PROPPARC.Where(w => w.NUMPARC == item.NUMPARC).Where(w => w.VERSAO == item.VERSAO).Where(w => w.STATUS == "A").ToList().ToArray();


                if (!string.IsNullOrEmpty(ownerentnum) && pl.parcelProperty.Length > 0)
                {
                    pl.parcelProperty = pl.parcelProperty.Where(w => w.ENTNUM.Trim() == ownerentnum.Trim()).ToList().ToArray();
                }

                if (pl.parcelExplorer.Length > 0 && pl.parcelProperty.Length > 0)
                {
                    if (!string.IsNullOrEmpty(entnum) && pl.parcelProperty.Length > 0)
                    {
                        pl.parcelProperty = pl.parcelProperty.Where(w => w.ENTNUM.Trim() == entnum.Trim()).ToList().ToArray();
                    }

                    if (!string.IsNullOrEmpty(entnum) && pl.parcelExplorer.Length > 0)
                    {
                        pl.parcelExplorer = pl.parcelExplorer.Where(w => w.ENTNUM.Trim() == entnum.Trim()).ToList().ToArray();
                    }
                }
                listdata.Add(pl);
            }
            return listdata;
        }



        public Parcel GetById(int id, short versao)
        {
            Parcel p = _context.Parcela.AsNoTracking().Where(w => w.NUMPARC == id).Where(w1 => w1.VERSAO == versao).ToList().FirstOrDefault();
            _context.SaveChanges();
            return p;
        }

        public MainParcelByID GetByAll(int id, short versao)
        {
            MainParcelByID mp = new MainParcelByID();
            mp.parcel = _context.Parcela.AsNoTracking().Where(w => w.NUMPARC == id).Where(w1 => w1.VERSAO == versao).Where(w2 => w2.STATUS == "A").ToList().FirstOrDefault();
            if (mp.parcel != null)
            {
                mp.parcelExplorer = _context.EXPLORPARC.AsNoTracking().Where(w => w.NUMPARC == id).Where(w1 => w1.VERSAO == versao).Where(w2 => w2.STATUS == "A").ToList().ToArray();
                mp.parcelProperty = _context.PROPPARC.AsNoTracking().Where(w => w.NUMPARC == id).Where(w1 => w1.VERSAO == versao).Where(w2 => w2.STATUS == "A").ToList().ToArray();
                mp.matrixArticle = _context.ARTIGO.AsNoTracking().Where(w => w.NUMPARC == id).Where(w1 => w1.VERSAO == versao).ToList().ToArray();
                LegalFramework[] legalFramework = null;
                legalFramework = _context.ENQLEGAL.AsNoTracking().Where(w => w.NUMPARC == id).Where(w1 => w1.VERSAO == versao).ToList().ToArray();
                mp.CASTAPARC = _context.CASTAPARC.AsNoTracking().Where(w => w.NUMPARC == id).Where(w1 => w1.VERSAO == versao).Where(w2 => w2.STATUS == "A").ToList().ToArray();

                List<LegalFrameworkList> alllist = new List<LegalFrameworkList>();
                foreach (LegalFramework item in legalFramework)
                {
                    LegalFrameworkList l = new LegalFrameworkList();
                    l.legalFrameworks = item;
                    l.Levantamento = _context.LEVANTAMENTO.AsNoTracking().Where(w => w.IDLEVANTAMENTO == item.IDENQLEGAL).ToList().ToArray();
                    alllist.Add(l);
                }
                mp.legalFramework = alllist.ToArray();
            }
            return mp;
        }

        public Parcel Create(MainParcel mp)
        {
            Parcel p = mp.parcel;
            p.VERSAO = 0;
            p.STATUS = "A";
            _context.Parcela.Add(p);
            _context.SaveChanges();

            foreach (LegalFramework item1 in mp.legalFramework)
            {
                if (item1 != null)
                {
                    LegalFramework lf = item1;
                    lf.NUMPARC = mp.parcel.NUMPARC;
                    lf.VERSAO = mp.parcel.VERSAO;
                    _context.ENQLEGAL.Add(lf);
                    _context.SaveChanges();
                }
            }

            foreach (MatrixArticle item1 in mp.matrixArticle)
            {
                if (item1 != null)
                {
                    MatrixArticle ma = item1;
                    ma.NUMPARC = mp.parcel.NUMPARC;
                    ma.VERSAO = mp.parcel.VERSAO;
                    _context.ARTIGO.Add(ma);
                    _context.SaveChanges();
                }
            }


            foreach (ParcelExplorer item1 in mp.parcelExplorer)
            {
                if (item1 != null)
                {
                    ParcelExplorer pe = item1;
                    pe.IDPARCENT = (_context.EXPLORPARC.OrderByDescending(u => u.IDPARCENT).FirstOrDefault().IDPARCENT) + 1;
                    pe.NUMPARC = mp.parcel.NUMPARC;
                    pe.VERSAO = mp.parcel.VERSAO;
                    _context.EXPLORPARC.Add(pe);
                    _context.SaveChanges();
                }
            }

            foreach (ParcelProperty item1 in mp.parcelProperty)
            {
                if (item1 != null)
                {
                    ParcelProperty pe = item1;
                    pe.NUMPARC = mp.parcel.NUMPARC;
                    pe.VERSAO = mp.parcel.VERSAO;
                    pe.STATUS = "A";
                    _context.PROPPARC.Add(pe);
                    _context.SaveChanges();
                }
            }

            foreach (CASTAPARC item1 in mp.CASTAPARC)
            {
                if (item1 != null)
                {
                    CASTAPARC pe = item1;
                    pe.NUMPARC = mp.parcel.NUMPARC;
                    pe.VERSAO = mp.parcel.VERSAO;
                    pe.STATUS = "A";
                    _context.CASTAPARC.Add(pe);
                    _context.SaveChanges();
                }
            }

            _context.SaveChanges();

            return p;
        }

        public void Update(MainParcel p)
        {
            if (p.parcel != null)
            {
                p.parcel.VERSAO = (short)(_context.Parcela.Where(w => w.NUMPARC == p.parcel.NUMPARC).Max(u => (short?)u.VERSAO) + 1);
                _context.Parcela.Add(p.parcel);
                _context.SaveChanges();
            }

            foreach (LegalFramework item1 in p.legalFramework)
            {
                if (item1 != null)
                {
                    LegalFramework lf = item1;
                    lf.NUMPARC = p.parcel.NUMPARC;
                    lf.VERSAO = p.parcel.VERSAO;
                    _context.ENQLEGAL.Add(lf);
                    _context.SaveChanges();
                }
            }

            foreach (MatrixArticle item1 in p.matrixArticle)
            {
                if (item1 != null)
                {
                    MatrixArticle ma = item1;
                    ma.NUMPARC = p.parcel.NUMPARC;
                    ma.VERSAO = p.parcel.VERSAO;
                    _context.ARTIGO.Add(ma);
                    _context.SaveChanges();
                }
            }
            foreach (ParcelExplorer item1 in p.parcelExplorer)
            {
                if (item1 != null)
                {
                    ParcelExplorer pe = item1;
                    pe.IDPARCENT = (_context.EXPLORPARC.OrderByDescending(u => u.IDPARCENT).FirstOrDefault().IDPARCENT) + 1;
                    pe.NUMPARC = p.parcel.NUMPARC;
                    pe.VERSAO = p.parcel.VERSAO;
                    _context.EXPLORPARC.Add(pe);
                    _context.SaveChanges();
                }
            }

            foreach (ParcelProperty item1 in p.parcelProperty)
            {
                if (item1 != null)
                {
                    ParcelProperty pe = item1;
                    pe.NUMPARC = p.parcel.NUMPARC;
                    pe.VERSAO = p.parcel.VERSAO;
                    _context.PROPPARC.Add(pe);
                    _context.SaveChanges();
                }
            }

            foreach (CASTAPARC item1 in p.CASTAPARC)
            {
                if (item1 != null)
                {
                    CASTAPARC pe = item1;
                    pe.NUMPARC = p.parcel.NUMPARC;
                    pe.VERSAO = p.parcel.VERSAO;
                    _context.CASTAPARC.Add(pe);
                    _context.SaveChanges();
                }
            }

            _context.SaveChanges();

            UpdateParcelStatus(p.parcel.NUMPARC, p.parcel.VERSAO);
        }


        public void UpdateParcelStatus(int NUMPARC, short VERSAO)
        {
            for (short i = 0; i < VERSAO; i++)
            {
                Parcel p1 = _context.Parcela.Where(w => w.NUMPARC == NUMPARC).Where(w => w.VERSAO == i).FirstOrDefault();
                _context.SaveChanges();
                p1.STATUS = "I";
                _context.Parcela.Update(p1);
                _context.SaveChanges();

                ParcelExplorer[] parcelExplorer = _context.EXPLORPARC.Where(w => w.NUMPARC == NUMPARC).Where(w => w.VERSAO == i).ToList().ToArray();
                _context.SaveChanges();
                foreach (ParcelExplorer item1 in parcelExplorer)
                {
                    if (item1 != null)
                    {
                        ParcelExplorer pe = item1;
                        pe.STATUS = "I";
                        _context.EXPLORPARC.Update(pe);
                        _context.SaveChanges();
                    }
                }

                ParcelProperty[] parcelProperty = _context.PROPPARC.Where(w => w.NUMPARC == NUMPARC).Where(w => w.VERSAO == i).ToList().ToArray();
                _context.SaveChanges();
                foreach (ParcelProperty item1 in parcelProperty)
                {
                    if (item1 != null)
                    {
                        ParcelProperty pe = item1;
                        pe.STATUS = "I";
                        _context.PROPPARC.Update(pe);
                        _context.SaveChanges();
                    }
                }

                CASTAPARC[] cASTAPARC = _context.CASTAPARC.Where(w => w.NUMPARC == NUMPARC).Where(w => w.VERSAO == i).ToList().ToArray();
                _context.SaveChanges();
                foreach (CASTAPARC item1 in cASTAPARC)
                {
                    if (item1 != null)
                    {
                        CASTAPARC pe = item1;
                        pe.STATUS = "I";
                        _context.CASTAPARC.Update(pe);
                        _context.SaveChanges();
                    }
                }
            }
        }

        public void Delete(int id, short versao)
        {
            var p = _context.Parcela.Where(w => w.NUMPARC == id).Where(w1 => w1.VERSAO == versao).ToList().FirstOrDefault();
            if (p != null)
            {
                p.STATUS = "P";
                _context.Parcela.Update(p);
                _context.SaveChanges();
            }
        }
    }
}
