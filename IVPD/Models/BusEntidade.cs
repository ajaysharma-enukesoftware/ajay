using System;
using System.ComponentModel.DataAnnotations;

namespace IVPD.Models
{
    public class BusEntidade
    {
        [Key]
        public int codEntidade { get; set; }
        public string nome { get; set; }
        public string nifap { get; set; }
        public int? codPais2nif { get; set; }
        public string nif { get; set; }
        public int? codNaturezaJuridica2 { get; set; }
        public int? codRegimeIva2 { get; set; }
        public int? codCae2 { get; set; }
        public string dataInicioAtividade { get; set; }
        public int? codCirs2 { get; set; }
        public int? codPais2nacionalidade { get; set; }
        public string bi { get; set; }
        public decimal? biValidade { get; set; }
        public string dataNascimento { get; set; }
        public string sexo { get; set; }
        public int? codPais2moradaFiscal { get; set; }
        public string moradaFiscal { get; set; }
        public string localFiscal { get; set; }
        public string localidadeFiscal { get; set; }
        public string codigoPostalFiscal1 { get; set; }
        public string codigoPostalFiscal2 { get; set; }
        public int? codNut2 { get; set; }
        public string formaObrigar { get; set; }
        public string codigoAcessoCartaoElec { get; set; }
        public int? codConservatoria2 { get; set; }
        public string dataMatriculaConservatoria { get; set; }
        public string dataEmissaoCRC { get; set; }
        public string codigoCertidaoPermanente { get; set; }
        public string dataSubscricaoConservatoria { get; set; }
        public string dataValidadeConservatoria { get; set; }
        public int? codTipoDiploma2 { get; set; }
        public string numDR { get; set; }
        public string anoDR { get; set; }
        public string registoDefinitivo { get; set; }
        public string numMaxAssinaturas { get; set; }
        public int? codDis { get; set; }
        public int? codCon { get; set; }
        public int? codConAux { get; set; }
        public string codFrg { get; set; }
        public bool? ivdp_validado { get; set; }
        public string ivdp_numero { get; set; }
        public string ivdp_designacaoReduzida { get; set; }
        public int? ivdp_codPaisEntidade { get; set; }
        public string ivdp_passaporte { get; set; }
        public decimal? ivdp_dataValidadePassaporte { get; set; }
        public string ivdp_observacoes { get; set; }
        public string ivdp_website { get; set; }
        public int? ivdp_codClassificacao { get; set; }
        public string ivdp_numIvv { get; set; }
        public string ivdp_numCasaDouro { get; set; }
        public int? ivdp_numIvp { get; set; }
        public string ivdp_moradaCorrLinha1 { get; set; }
        public string ivdp_moradaCorrLinha2 { get; set; }
        public int? ivdp_moradaCorrCodigoPostal1 { get; set; }
        public int? ivdp_moradaCorrCodigoPostal2 { get; set; }
        public string ivdp_moradaCorrLocalidade { get; set; }
        public bool? importadoIfap { get; set; }
        public bool? ivdp_ativo { get; set; }
    }
    
    public class BusEntityList
    {
        [Key]
        public int codEntidade { get; set; }
        public string nome { get; set; }
        public string nifap { get; set; }
        public int? codPais2nif { get; set; }
        public string nif { get; set; }
    }
}
