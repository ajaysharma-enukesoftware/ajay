using IVPD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Services
{
    public interface IFatoresPontucaoService
    {
       public Parcel GetCalculations(int? parcelId, string geoCod);
    }
    public class FatoresPontucaoService : IFatoresPontucaoService
    {
        private IVPDContext _context;

        public FatoresPontucaoService(IVPDContext context)
        {
            _context = context;
        }

        public Parcel GetCalculations(int? parcelId, string geoCod)
        {

            //hit webservice and store response on param
            //   Parcel p = _context.Parcela.Where(w => w.NUMPARC == parcelId).FirstOrDefault();
            /* FatoresPontucaoFields param = new FatoresPontucaoFields();
             param.PontuacaoAltitude = "1234";
             Parcel par = new Parcel();
             par.FPALTITUDE = Convert.ToInt32(param.PontuacaoAltitude);
             par.FPIDADEVINHA =
                 Int32.Parse(param.PontuacaoExposicao_E + param.PontuacaoExposicao_ENE + 
                 param.PontuacaoExposicao_ESE + param.PontuacaoExposicao_NE + param.PontuacaoExposicao_NNE + 
                 param.PontuacaoExposicao_NNO + param.PontuacaoExposicao_NO + param.PontuacaoExposicao_N + 
                 param.PontuacaoExposicao_O + param.PontuacaoExposicao_ONO + param.PontuacaoExposicao_OSO + 
                 param.PontuacaoExposicao_S + param.PontuacaoExposicao_SE + param.PontuacaoExposicao_SO + 
                 param.PontuacaoExposicao_SSE + param.PontuacaoExposicao_SSO);

             par.FPINCLINACAO = Int32.Parse(param.PontuacaoInclinacao);

             par.FPNATURTERRENO = Convert.ToInt32(param.PontuacaoUsoSolo_FundosFerteis) + Convert.ToInt32(param.PontuacaoUsoSolo_Granito) +
                 Convert.ToInt32(param.PontuacaoUsoSolo_Transicao) + Convert.ToInt32(param.PontuacaoUsoSolo_Xisto);
             par.FPABRIGO = Convert.ToInt32(param.PontuacaoAbrigo);
             par.FPPRODUTIVIDADE = 120;
             par.FPCOMPASSO = 50;
             par.FPARMACAO = 100;
             double ptPedregosidade = 0.0;
             if(param.Pedregosidade!=null)
             {

             }
             par.FPPEDREGOSIDADE = Convert.ToInt32(ptPedregosidade);
             double ptCastas = 0.0;
             if (param.Pedregosidade != null)
             {

             }
             par.FPCASTAS = Convert.ToInt32(ptCastas);
              par.FPIDADEVINHA= Convert.ToInt32(param.PontuacaoIdadeVinha);
            */
            if (parcelId == null)
            {
                FatoresPontucaoFields param = new FatoresPontucaoFields();
                param.pontuacaoAltitudeField = 1234;
                param.pontuacaoExposicao_SOField = 2123;
                param.pontuacaoExposicao_OSOField = 2312;
                param.pontuacaoExposicao_NOField = 323;
                param.pontuacaoExposicao_NField = 2321;
                param.pontuacaoExposicao_ENEField = 45438;
                param.pontuacaoExposicao_EField = 423;
                param.pontuacaoAbrigoField = 3232;
                param.percentagem_GranitoField = 3132;
                param.percentagem_FundosFerteisField = 32432;
                param.intervaloIdadeVinhaSuperiorField = 332;
                param.iDAbrigoField = 232;
                param.exposicao_SSOField = 32223;
                param.inclinacaoMediaField = 232;
                param.intervaloIdadeVinhaInferiorField = 2321;
                param.nomeParcelaField = "2122";
                Parcel par = new Parcel();
          

                    par.FPALTITUDE = Convert.ToInt32(param.pontuacaoAltitudeField);
                par.FPIDADEVINHA =
                    (Int32)(param.pontuacaoExposicao_EField + param.pontuacaoExposicao_ENEField + param.pontuacaoExposicao_ESEField + param.pontuacaoExposicao_NEField + param.pontuacaoExposicao_NNEField +
                    param.pontuacaoExposicao_NNOField + param.pontuacaoExposicao_NOField + param.pontuacaoExposicao_NField +
                    param.pontuacaoExposicao_OField + param.pontuacaoExposicao_ONOField + param.pontuacaoExposicao_OSOField +
                    param.pontuacaoExposicao_SField + param.pontuacaoExposicao_SEField + param.pontuacaoExposicao_SOField +
                    param.pontuacaoExposicao_SSEField + param.pontuacaoExposicao_SSOField);

                par.FPINCLINACAO = Convert.ToInt32(param.pontuacaoInclinacaoField);

                par.FPNATURTERRENO = Convert.ToInt32(param.pontuacaoUsoSolo_FundosFerteisField) + Convert.ToInt32(param.pontuacaoUsoSolo_GranitoField) +
                    Convert.ToInt32(param.pontuacaoUsoSolo_TransicaoField) + Convert.ToInt32(param.pontuacaoUsoSolo_XistoField);
                par.FPABRIGO = Convert.ToInt32(param.pontuacaoAbrigoField);
                par.FPPRODUTIVIDADE = 120;
                par.FPCOMPASSO = 50;
                par.FPARMACAO = 100;
                double ptPedregosidade = 0.0;
                if (param.pedregosidadeField != null)
                {
                    foreach (ElementoPontuacao item in param.pedregosidadeField)
                    {
                        ptPedregosidade += item.pontuacaoField;
                    }

                }
                par.FPPEDREGOSIDADE = Convert.ToInt32(ptPedregosidade);
                double ptCastas = 0.0;
                if (param.pedregosidadeField != null)
                {
                    foreach (ElementoPontuacao item in param.pedregosidadeField)
                    {
                        ptCastas += item.pontuacaoField;
                    }

                }
                par.FPCASTAS = Convert.ToInt32(ptCastas);
                par.FPIDADEVINHA = Convert.ToInt32(param.pontuacaoIdadeVinhaField);
                return par;
            }
            else
            {
                FatoresPontucaoFields param = new FatoresPontucaoFields();
                param.pontuacaoAltitudeField = 1234;
                param.pontuacaoExposicao_SOField = 2123;
                param.pontuacaoExposicao_OSOField = 2312;
                param.pontuacaoExposicao_NOField = 323;
                param.pontuacaoExposicao_NField = 2321;
                param.pontuacaoExposicao_ENEField = 45438;
                param.pontuacaoExposicao_EField = 423;
                param.pontuacaoAbrigoField = 3232;
                param.percentagem_GranitoField = 3132;
                param.percentagem_FundosFerteisField = 32432;
                param.intervaloIdadeVinhaSuperiorField = 332;
                param.iDAbrigoField = 232;
                param.exposicao_SSOField = 32223;
                param.inclinacaoMediaField = 232;
                param.intervaloIdadeVinhaInferiorField = 2321;
                param.nomeParcelaField = "2122";
                Parcel par = new Parcel();
                par.FPALTITUDE = Convert.ToInt32(param.pontuacaoAltitudeField);
                par.FPEXPOSICAO =
                    (Int32)(param.pontuacaoExposicao_EField + param.pontuacaoExposicao_ENEField + param.pontuacaoExposicao_ESEField + param.pontuacaoExposicao_NEField + param.pontuacaoExposicao_NNEField +
                    param.pontuacaoExposicao_NNOField + param.pontuacaoExposicao_NOField + param.pontuacaoExposicao_NField +
                    param.pontuacaoExposicao_OField + param.pontuacaoExposicao_ONOField + param.pontuacaoExposicao_OSOField +
                    param.pontuacaoExposicao_SField + param.pontuacaoExposicao_SEField + param.pontuacaoExposicao_SOField +
                    param.pontuacaoExposicao_SSEField + param.pontuacaoExposicao_SSOField);

                par.FPINCLINACAO = Convert.ToInt32(param.pontuacaoInclinacaoField);

                par.FPNATURTERRENO = Convert.ToInt32(param.pontuacaoUsoSolo_FundosFerteisField) + Convert.ToInt32(param.pontuacaoUsoSolo_GranitoField) +
                    Convert.ToInt32(param.pontuacaoUsoSolo_TransicaoField) + Convert.ToInt32(param.pontuacaoUsoSolo_XistoField);
                par.FPABRIGO = Convert.ToInt32(param.pontuacaoAbrigoField);
                par.FPPRODUTIVIDADE = 120;
                par.FPCOMPASSO = 50;
                par.FPARMACAO = 100;
                double ptPedregosidade = 0.0;
                if (param.pedregosidadeField != null)
                {
                    foreach (ElementoPontuacao item in param.pedregosidadeField)
                    {
                        ptPedregosidade += item.pontuacaoField;
                    }

                }
                par.FPPEDREGOSIDADE = Convert.ToInt32(ptPedregosidade);
                double ptCastas = 0.0;
                if (param.pedregosidadeField != null)
                {
                    foreach (ElementoPontuacao item in param.pedregosidadeField)
                    {
                        ptCastas += item.pontuacaoField;
                    }

                }
                par.FPCASTAS = Convert.ToInt32(ptCastas);
                par.FPIDADEVINHA = Convert.ToInt32(param.pontuacaoIdadeVinhaField);
                return par;
            }

        }
    }
}
