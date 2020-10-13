using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Models
{
    public class FatoresPontucao
    {/*
        public  string PontuacaoAltitude();
        public string PontuacaoExposicao_E { get; set; }
        public string PontuacaoExposicao_ENE { get; set; }
        public string PontuacaoExposicao_ESE { get; set; }
        public string PontuacaoExposicao_NE { get; set; }
        public string PontuacaoExposicao_NNE { get; set; }
        public string PontuacaoExposicao_NNO { get; set; }
        public string PontuacaoExposicao_NO { get; set; }
        public string PontuacaoExposicao_N { get; set; }
        public string PontuacaoExposicao_O { get; set; }
        public string PontuacaoExposicao_ONO { get; set; }
        public string PontuacaoExposicao_OSO { get; set; }
        public string PontuacaoExposicao_S { get; set; }
        public string PontuacaoExposicao_SE { get; set; }
        public string PontuacaoExposicao_SO { get; set; }
        public string PontuacaoExposicao_SSE { get; set; }
        public string PontuacaoExposicao_SSO { get; set; }
        public string PontuacaoInclinacao { get; set; }
        public string PontuacaoUsoSolo_FundosFerteis { get; set; }
        public string PontuacaoUsoSolo_Granito { get; set; }
        public string PontuacaoUsoSolo_Transicao { get; set; }
        public string PontuacaoUsoSolo_Xisto { get; set; }
        public string PontuacaoAbrigo { get; set; }
        public string Pedregosidade { get; set; }
        public string PontuacaoIdadeVinha { get; set; }

        */
    }
    public class FatoresPontucaoFields
    {
        public DateTime dataClassificacaoField { get; set; }

        public string dICOFREField { get; set; }

        public string codigoLocalizacaoField { get; set; }

        public Double pontuacaoMinimaLocalizacaoField { get; set; }

        public Double pontuacaoMediaLocalizacaoField { get; set; }

        public Double pontuacaoMaximaLocalizacaoField { get; set; }

        public Double inclinacaoMediaField { get; set; }

        public Double pontuacaoInclinacaoField { get; set; }

        public Double exposicao_NField { get; set; }

        public Double exposicao_SField { get; set; }

        public Double exposicao_EField { get; set; }

        public Double exposicao_OField { get; set; }

        public Double exposicao_NEField { get; set; }

        public Double exposicao_NOField { get; set; }

        public Double exposicao_SEField { get; set; }

        public Double exposicao_SOField { get; set; }

        public Double exposicao_NNEField { get; set; }

        public Double exposicao_NNOField { get; set; }

        public Double exposicao_SSEField { get; set; }

        public Double exposicao_SSOField { get; set; }

        public Double exposicao_ESEField { get; set; }

        public Double exposicao_ENEField { get; set; }

        public Double exposicao_ONOField { get; set; }

        public Double exposicao_OSOField { get; set; }

        public Double pontuacaoExposicao_NField { get; set; }

        public Double pontuacaoExposicao_SField { get; set; }

        public Double pontuacaoExposicao_EField { get; set; }

        public Double pontuacaoExposicao_OField { get; set; }

        public Double pontuacaoExposicao_NEField { get; set; }

        public Double pontuacaoExposicao_NOField { get; set; }

        public Double pontuacaoExposicao_SEField { get; set; }

        public Double pontuacaoExposicao_SOField { get; set; }

        public Double pontuacaoExposicao_NNEField { get; set; }

        public Double pontuacaoExposicao_NNOField { get; set; }

        public Double pontuacaoExposicao_SSEField { get; set; }

        public Double pontuacaoExposicao_SSOField { get; set; }

        public Double pontuacaoExposicao_ESEField { get; set; }

        public Double pontuacaoExposicao_ENEField { get; set; }

        public Double pontuacaoExposicao_ONOField { get; set; }

        public Double pontuacaoExposicao_OSOField { get; set; }

        public Double altituteMinimaField { get; set; }

        public Double altitudeMediaField { get; set; }

        public Double altitudeMaximaField { get; set; }

        public Double pontuacaoAltitudeField { get; set; }

        public Double percentagem_XistoField { get; set; }

        public Double percentagem_TransicaoField { get; set; }

        public Double percentagem_GranitoField { get; set; }

        public Double percentagem_FundosFerteisField { get; set; }

        public Double pontuacaoUsoSolo_XistoField { get; set; }

        public Double pontuacaoUsoSolo_TransicaoField { get; set; }

        public Double pontuacaoUsoSolo_GranitoField { get; set; }

        public Double pontuacaoUsoSolo_FundosFerteisField { get; set; }

        public Double intervaloIdadeVinhaInferiorField { get; set; }

        public Double intervaloIdadeVinhaSuperiorField { get; set; }

        public Double pontuacaoIdadeVinhaField { get; set; }

        public List<ElementoPontuacao> castasField { get; set; }

        public List<ElementoPontuacao> pedregosidadeField { get; set; }

        public Double iDAbrigoField { get; set; }

        public Double pontuacaoAbrigoField { get; set; }

        public Double anoPlantacaoField { get; set; }

        public Double compassoLinhaField { get; set; }

        public Double compassoEntreLinhaField { get; set; }

        
        //    Private proprietariosField() { get; set; }

        //    Private direitosParcelaField() As DireitoParcela

        public Double pontuacaoClasseField { get; set; }

        public Double areaParcelaField { get; set; }

        public String nomeParcelaField { get; set; }

    }
    public class ElementoPontuacao
{  public String idField { get; set; }

    public String nomeField { get; set; }

    public Double percentagemField { get; set; }

    public Double pontuacaoField { get; set; }
}
public class APIFatoresPontucaoCalculationRequest
    {
        public int? parcelId { get; set; }
        public string geocod { get; set; }

    }
}
