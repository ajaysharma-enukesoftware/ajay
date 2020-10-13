using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace IVPD.Models
{
    [Table("PARCELA", Schema = "Parcela")]
    public partial class Parcel : IValidatableObject
    {
        public int NUMPARC { get; set; }
        public short VERSAO { get; set; }

        [Required]
        [DisplayName("IDSITLEG")]
        public byte IDSITLEG { get; set; }
        public string CODLITIGIO { get; set; }

        [Required]
        public int CODDIS { get; set; }

        [Required]
        public int CODCON { get; set; }

        [Required]
        public int CODFRG { get; set; }

        [Required]
        public byte IDSITPARC { get; set; }
        public decimal? CONCFR { get; set; }
        public decimal? NUMPARANT { get; set; }

        [MaxLength(12)]
        public string GEOCOD { get; set; }

        [MaxLength(12)]
        public string TITDIREITO { get; set; }

        [Required]
        [MaxLength(55)]
        public string DESPARC { get; set; }

        [Required]
        [MaxLength(1)]
        public string CLASSE { get; set; }

        [Required]
        public decimal PESILEGAIS { get; set; }

        [Required]
        public decimal PESMAIOR3 { get; set; }

        [Required]
        public decimal PESMENOR3 { get; set; }

        [Required]
        public decimal PESRAMLEG { get; set; }

        [Required]
        public decimal PESFALILEG { get; set; }

        [Required]
        public decimal PESFALLEG { get; set; }

        [Required]
        public decimal FALHASRAM { get; set; }

        [Required]
        public decimal PCTTINTA { get; set; }

        [Required]
        public decimal PCTBRANCA { get; set; }

        [Required]
        public decimal PCTROSE { get; set; }

        [Required]
        public decimal PCTMOSCATEL { get; set; }

        [Required]
        public decimal PCTDOPORTO { get; set; }

        [Required]
        public decimal COMPASSO { get; set; }

        [Required]
        public decimal AREAAPTA { get; set; }

        [Required]
        public decimal AREANAPTA { get; set; }

        [Required]
        public decimal AREAILEGAL { get; set; }

        [Required]
        public decimal PRODBRIG { get; set; }

        [Required]
        public decimal PONTUACAO { get; set; }

        [MaxLength(1)]
        public string COMMOSTO { get; set; }
        public decimal? DTULTVIST { get; set; }

        [Required]
        public bool SITDECLAR { get; set; }

        [Required]
        public bool AREAPROJEC { get; set; }

        [Required]
        public bool ORIGEMMCP { get; set; }

        [Required]
        [MaxLength(1)]
        public string STATUS { get; set; }

        [Required]
        public decimal DTACT { get; set; }

        [Required]
        public decimal HRACT { get; set; }

        [Required]
        [MaxLength(25)]
        public string USR { get; set; }

        [Required]
        public string WKS { get; set; }
        public decimal? ANOPLANTACAO { get; set; }

        [Required]
        public int BACELOS { get; set; }

        [Required]
        public int ENXERTOS1 { get; set; }

        [Required]
        public int VIDEIRAS2 { get; set; }

        [Required]
        public int VIDEIRAS3 { get; set; }

        [Required]
        public int VIDEIRAS4A25 { get; set; }

        [Required]
        public int VIDEIRASMAIOR25 { get; set; }

        [Required]
        public int BACELOSILEG { get; set; }

        [Required]
        public int ENXERTOS1ILEG { get; set; }

        [Required]
        public int VIDEIRAS2ILEG { get; set; }

        [Required]
        public int VIDEIRAS3ILEG { get; set; }

        [Required]
        public int VIDEIRAS4A25ILEG { get; set; }

        [Required]
        public int VIDEIRASMAIOR25ILEG { get; set; }

        [Required]
        public int FPLOCALIZACAO { get; set; }

        [Required]
        public int FPALTITUDE { get; set; }

        [Required]
        public int FPABRIGO { get; set; }

        [Required]
        public int FPINCLINACAO { get; set; }

        [Required]
        public int FPEXPOSICAO { get; set; }

        [Required]
        public int FPCASTAS { get; set; }

        [Required]
        public int FPPEDREGOSIDADE { get; set; }

        [Required]
        public int FPNATURTERRENO { get; set; }

        [Required]
        public int FPARMACAO { get; set; }

        [Required]
        public int FPCOMPASSO { get; set; }

        [Required]
        public int FPPRODUTIVIDADE { get; set; }

        [Required]
        public int FPIDADEVINHA { get; set; }


        public int? IDCLASSEPLANT { get; set; }
        public bool? LEGALIZAVEL { get; set; }

        [Required]
        public int PESCASTASNAOAPTAS { get; set; }

        [Required]
        public int PESSEMARAMACAO { get; set; }
        public int? IDTPESTADOVINHA { get; set; }
        public int? IDTPARMTERRENO { get; set; }

        [Required]
        public bool PAPV { get; set; }

        [Required]
        public bool PORTAL { get; set; }

        [Required]
        public decimal DOPORTOCASTAS { get; set; }
        public int? fatoraptidao { get; set; }
        public int? pestotals { get; set; }
        public int? areaeffective { get; set; }
        public bool? ignorvalid { get; set; }
        public int? areaeqlegal { get; set; }
        public int? areapotential { get; set; }
        public bool? areaprojectada { get; set; }
        public string armacaoterreno { get; set; }
        public int? fatorporto { get; set; }
        public int? prctLss3AndFailures { get; set; }
        public int? prctfaults { get; set; }
        public int? densidade { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (IDSITLEG == 0)
                yield return new ValidationResult("IDSITLEG field is required!", new[] { nameof(IDSITLEG) });

            if (CODDIS == 0)
                yield return new ValidationResult("CODDIS field is required!", new[] { nameof(CODDIS) });

            if (CODCON == 0)
                yield return new ValidationResult("CODDIS field is required!", new[] { nameof(CODCON) });

            if (CODFRG == 0)
                yield return new ValidationResult("CODFRG field is required!", new[] { nameof(CODFRG) });

            if (string.IsNullOrEmpty(DESPARC))
                yield return new ValidationResult("DESPARC field is required!", new[] { nameof(DESPARC) });

            if (string.IsNullOrEmpty(CLASSE))
                yield return new ValidationResult("CLASSE field is required!", new[] { nameof(CLASSE) });

            if (PESILEGAIS == 0)
                yield return new ValidationResult("PESILEGAIS field is required!", new[] { nameof(PESILEGAIS) });

            if (PESMAIOR3 == 0)
                yield return new ValidationResult("PESMAIOR3 field is required!", new[] { nameof(PESMAIOR3) });

            if (PESMENOR3 == 0)
                yield return new ValidationResult("PESMENOR3 field is required!", new[] { nameof(PESMENOR3) });

            if (PESRAMLEG == 0)
                yield return new ValidationResult("PESRAMLEG field is required!", new[] { nameof(PESRAMLEG) });

            if (PESFALILEG == 0)
                yield return new ValidationResult("PESFALILEG field is required!", new[] { nameof(PESFALILEG) });

            if (PESFALLEG == 0)
                yield return new ValidationResult("PESFALLEG field is required!", new[] { nameof(PESFALLEG) });

            if (FALHASRAM == 0)
                yield return new ValidationResult("FALHASRAM field is required!", new[] { nameof(FALHASRAM) });

            if (PCTTINTA == 0)
                yield return new ValidationResult("PCTTINTA field is required!", new[] { nameof(PCTTINTA) });

            if (PCTBRANCA == 0)
                yield return new ValidationResult("PCTBRANCA field is required!", new[] { nameof(PCTBRANCA) });

            if (PCTROSE == 0)
                yield return new ValidationResult("PCTROSE field is required!", new[] { nameof(PCTROSE) });

            if (PCTMOSCATEL == 0)
                yield return new ValidationResult("PCTMOSCATEL field is required!", new[] { nameof(PCTMOSCATEL) });

            if (PCTDOPORTO == 0)
                yield return new ValidationResult("PCTDOPORTO field is required!", new[] { nameof(PCTDOPORTO) });

            if (COMPASSO == 0)
                yield return new ValidationResult("COMPASSO field is required!", new[] { nameof(COMPASSO) });

            if (AREAAPTA == 0)
                yield return new ValidationResult("AREAAPTA field is required!", new[] { nameof(AREAAPTA) });

            if (AREANAPTA == 0)
                yield return new ValidationResult("AREANAPTA field is required!", new[] { nameof(AREANAPTA) });

            if (AREAILEGAL == 0)
                yield return new ValidationResult("AREAILEGAL field is required!", new[] { nameof(AREAILEGAL) });

            if (PRODBRIG == 0)
                yield return new ValidationResult("PRODBRIG field is required!", new[] { nameof(PRODBRIG) });

            if (PONTUACAO == 0)
                yield return new ValidationResult("PONTUACAO field is required!", new[] { nameof(PONTUACAO) });

            if (DTACT == 0)
                yield return new ValidationResult("DTACT field is required!", new[] { nameof(DTACT) });

            if (HRACT == 0)
                yield return new ValidationResult("HRACT field is required!", new[] { nameof(HRACT) });

            if (string.IsNullOrEmpty(USR))
                yield return new ValidationResult("USR field is required!", new[] { nameof(USR) });

            if (string.IsNullOrEmpty(WKS))
                yield return new ValidationResult("WKS field is required!", new[] { nameof(WKS) });

            if (BACELOS == 0)
                yield return new ValidationResult("BACELOS field is required!", new[] { nameof(BACELOS) });

            if (ENXERTOS1 == 0)
                yield return new ValidationResult("ENXERTOS1 field is required!", new[] { nameof(ENXERTOS1) });

            if (VIDEIRAS2 == 0)
                yield return new ValidationResult("VIDEIRAS2 field is required!", new[] { nameof(VIDEIRAS2) });

            if (VIDEIRAS3 == 0)
                yield return new ValidationResult("VIDEIRAS3 field is required!", new[] { nameof(VIDEIRAS3) });

            if (VIDEIRAS4A25 == 0)
                yield return new ValidationResult("VIDEIRAS4A25 field is required!", new[] { nameof(VIDEIRAS4A25) });

            if (VIDEIRASMAIOR25 == 0)
                yield return new ValidationResult("VIDEIRASMAIOR25 field is required!", new[] { nameof(VIDEIRASMAIOR25) });

            //if (BACELOSILEG == 0)
            //    yield return new ValidationResult("BACELOSILEG field is required!", new[] { nameof(BACELOSILEG) });

            //if (ENXERTOS1ILEG == 0)
            //    yield return new ValidationResult("ENXERTOS1ILEG field is required!", new[] { nameof(ENXERTOS1ILEG) });

            //if (VIDEIRAS2ILEG == 0)
            //    yield return new ValidationResult("VIDEIRAS2ILEG field is required!", new[] { nameof(VIDEIRAS2ILEG) });

            //if (VIDEIRAS3ILEG == 0)
            //    yield return new ValidationResult("VIDEIRAS3ILEG field is required!", new[] { nameof(VIDEIRAS3ILEG) });

            //if (VIDEIRAS4A25ILEG == 0)
            //    yield return new ValidationResult("VIDEIRAS4A25ILEG field is required!", new[] { nameof(VIDEIRAS4A25ILEG) });

            //if (VIDEIRASMAIOR25ILEG == 0)
            //    yield return new ValidationResult("VIDEIRASMAIOR25ILEG field is required!", new[] { nameof(VIDEIRASMAIOR25ILEG) });

            if (FPLOCALIZACAO == 0)
                yield return new ValidationResult("FPLOCALIZACAO field is required!", new[] { nameof(FPLOCALIZACAO) });

            if (FPALTITUDE == 0)
                yield return new ValidationResult("FPALTITUDE field is required!", new[] { nameof(FPALTITUDE) });

            if (FPABRIGO == 0)
                yield return new ValidationResult("FPABRIGO field is required!", new[] { nameof(FPABRIGO) });

            if (FPINCLINACAO == 0)
                yield return new ValidationResult("FPINCLINACAO field is required!", new[] { nameof(FPINCLINACAO) });

            if (FPEXPOSICAO == 0)
                yield return new ValidationResult("FPEXPOSICAO field is required!", new[] { nameof(FPEXPOSICAO) });

            if (FPCASTAS == 0)
                yield return new ValidationResult("FPCASTAS field is required!", new[] { nameof(FPCASTAS) });

            if (FPPEDREGOSIDADE == 0)
                yield return new ValidationResult("FPPEDREGOSIDADE field is required!", new[] { nameof(FPPEDREGOSIDADE) });

            if (FPNATURTERRENO == 0)
                yield return new ValidationResult("FPNATURTERRENO field is required!", new[] { nameof(FPNATURTERRENO) });

            if (FPARMACAO == 0)
                yield return new ValidationResult("FPARMACAO field is required!", new[] { nameof(FPARMACAO) });

            if (FPCOMPASSO == 0)
                yield return new ValidationResult("FPCOMPASSO field is required!", new[] { nameof(FPCOMPASSO) });

            if (FPPRODUTIVIDADE == 0)
                yield return new ValidationResult("FPPRODUTIVIDADE field is required!", new[] { nameof(FPPRODUTIVIDADE) });

            if (FPIDADEVINHA == 0)
                yield return new ValidationResult("FPIDADEVINHA field is required!", new[] { nameof(FPIDADEVINHA) });

            if (PESCASTASNAOAPTAS == 0)
                yield return new ValidationResult("PESCASTASNAOAPTAS field is required!", new[] { nameof(PESCASTASNAOAPTAS) });

            if (PESSEMARAMACAO == 0)
                yield return new ValidationResult("PESSEMARAMACAO field is required!", new[] { nameof(PESSEMARAMACAO) });

            if (DOPORTOCASTAS == 0)
                yield return new ValidationResult("DOPORTOCASTAS field is required!", new[] { nameof(DOPORTOCASTAS) });
        }
    }

    [Table("ENQLEGAL", Schema = "Parcela")]
    public class LegalFramework : IValidatableObject
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int IDENQLEGAL { get; set; }

        [Required]
        [MaxLength(12)]
        public string IDDTPLANTACAO { get; set; }

        [MaxLength(12)]
        public string IDDTORIGEM { get; set; }

        [MaxLength(1000)]
        public string TIPO { get; set; }

        [MaxLength(500)]
        public string PREDIOARRANQUE { get; set; }

        [Required]
        public int NUMPARC { get; set; }

        [Required]
        public short VERSAO { get; set; }
        public decimal? AREAAPDOURO { get; set; }
        public decimal? AREAAPPORTO { get; set; }

        [MaxLength(1000)]
        public string OBS { get; set; }

        [Required]
        public bool DISPONIVEL { get; set; }

        [Required]
        public DateTime DTINSERT { get; set; }

        [Required]
        [MaxLength(25)]
        public string USR { get; set; }

        [Required]
        [MaxLength(1000)]
        public string TIPOHA { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(IDDTPLANTACAO))
            {
                yield return new ValidationResult("IDDTPLANTACAO field is required!", new[] { nameof(IDDTPLANTACAO) });
            }

            if (string.IsNullOrEmpty(USR))
            {
                yield return new ValidationResult("USR field is required!", new[] { nameof(USR) });
            }

            if (string.IsNullOrEmpty(TIPOHA))
            {
                yield return new ValidationResult("TIPOHA field is required!", new[] { nameof(TIPOHA) });
            }
        }
    }

    [Table("ARTIGO", Schema = "Parcela")]
    public class MatrixArticle : IValidatableObject
    {
        [Key]
        public int IDARTIGO { get; set; }

        [Required]
        public int NUMPARC { get; set; }

        [Required]
        public short VERSAO { get; set; }

        [Required]
        public int NRARTIGO { get; set; }

        [MaxLength(1)]
        public string FRACCAO { get; set; }

        [Required]
        public DateTime DTACT { get; set; }

        [Required]
        [MaxLength(25)]
        public string USR { get; set; }

        [Required]
        [MaxLength(25)]
        public string WKS { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (NRARTIGO == 0)
            {
                yield return new ValidationResult("NRARTIGO field is required!", new[] { nameof(NRARTIGO) });
            }

            if (string.IsNullOrEmpty(USR))
            {
                yield return new ValidationResult("USR field is required!", new[] { nameof(USR) });
            }

            if (string.IsNullOrEmpty(WKS))
            {
                yield return new ValidationResult("WKS field is required!", new[] { nameof(WKS) });
            }
        }
    }

    [Table("Propparc", Schema = "Parcela")]
    public class ParcelProperty : IValidatableObject
    {
        [Key]
        public int IDPROPPARC { get; set; }

        [ForeignKey("NUMPARC")]
        [Required]
        public int NUMPARC { get; set; }

        [ForeignKey("VERSAO")]
        [Required]
        public short VERSAO { get; set; }

        [Required]
        public string ENTNUM { get; set; }

        [Required]
        [MaxLength(1)]
        public string STATUS { get; set; }
        public decimal? DTINICIO { get; set; }
        public decimal? DTFIM { get; set; }
        public int? DIVIDENDO { get; set; }
        public int? DIVISOR { get; set; }

        [Required]
        public decimal DTACT { get; set; }

        [Required]
        public decimal HRACT { get; set; }

        [Required]
        [MaxLength(25)]
        public string USR { get; set; }

        [Required]
        [MaxLength(25)]
        public string WKS { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DTACT == 0)
            {
                yield return new ValidationResult("DTACT field is required!", new[] { nameof(DTACT) });
            }

            if (HRACT == 0)
            {
                yield return new ValidationResult("HRACT field is required!", new[] { nameof(HRACT) });
            }

            if (string.IsNullOrEmpty(USR))
            {
                yield return new ValidationResult("USR field is required!", new[] { nameof(USR) });
            }

            if (string.IsNullOrEmpty(WKS))
            {
                yield return new ValidationResult("WKS field is required!", new[] { nameof(WKS) });
            }
        }
    }

    [Table("EXPLORPARC", Schema = "Parcela")]
    public class ParcelExplorer : IValidatableObject
    {
        [Key]
        public int IDPARCENT { get; set; }

        [Required]
        public int NUMPARC { get; set; }

        [Required]
        public short VERSAO { get; set; }


        [Required]
        [MaxLength(12)]
        public string ENTNUM { get; set; }
        public decimal? ANOPROC { get; set; }
        public int? NRPROC { get; set; }
        public decimal? AREAAPTA { get; set; }
        public decimal? AREANAPTA { get; set; }
        public decimal? AREAAPTAF { get; set; }
        public decimal? AREAAF { get; set; }
        public decimal? AREANAPTAF { get; set; }
        public decimal? AREAILEGAL { get; set; }
        public decimal? PCTDOPORTO { get; set; }
        public int? DIVIDENDO { get; set; }
        public int? DIVISOR { get; set; }

        [MaxLength(1)]
        public string SITDCP { get; set; }
        public decimal? ANODCP { get; set; }
        public decimal? DTINICIO { get; set; }
        public decimal? DTFIM { get; set; }

        [Required]
        [MaxLength(1)]
        public string STATUS { get; set; }

        [Required]
        public decimal DTACT { get; set; }

        [Required]
        public decimal HRACT { get; set; }

        [Required]
        [MaxLength(25)]
        public string USR { get; set; }

        [Required]
        [MaxLength(25)]
        public string WKS { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (string.IsNullOrEmpty(ENTNUM))
            {
                yield return new ValidationResult("ENTNUM field is required!", new[] { nameof(ENTNUM) });
            }
            if (DTACT == 0)
            {
                yield return new ValidationResult("DTACT field is required!", new[] { nameof(DTACT) });
            }

            if (HRACT == 0)
            {
                yield return new ValidationResult("HRACT field is required!", new[] { nameof(HRACT) });
            }

            if (string.IsNullOrEmpty(USR))
            {
                yield return new ValidationResult("USR field is required!", new[] { nameof(USR) });
            }

            if (string.IsNullOrEmpty(WKS))
            {
                yield return new ValidationResult("WKS field is required!", new[] { nameof(WKS) });
            }
        }
    }

    [Table("CASTAPARC", Schema = "Parcela")]
    public class CASTAPARC : IValidatableObject
    {
        [Key]
        public int IDCASTPARC { get; set; }

        [Required]
        public int NUMPARC { get; set; }

        [Required]
        public short VERSAO { get; set; }

        [Required]
        public int CODCASTA { get; set; }
        public decimal? PCTCASTA { get; set; }


        [Required]
        [MaxLength(1)]
        public string STATUS { get; set; }

        [Required]
        public decimal DTACT { get; set; }

        [Required]
        public decimal HRACT { get; set; }

        [Required]
        [MaxLength(25)]
        public string USR { get; set; }

        [Required]
        [MaxLength(25)]
        public string WKS { get; set; }
        public string douroPort { get; set; }
        public int? idcor { get; set; }
        public string idsinonimo { get; set; }
        public int? varietiesFactor { get; set; }
        public int? portFactor { get; set; }
        public int? st { get; set; }
        public string Estado { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (CODCASTA == 0)
            {
                yield return new ValidationResult("CODCASTA field is required!", new[] { nameof(CODCASTA) });
            }
            if (DTACT == 0)
            {
                yield return new ValidationResult("DTACT field is required!", new[] { nameof(DTACT) });
            }

            if (HRACT == 0)
            {
                yield return new ValidationResult("HRACT field is required!", new[] { nameof(HRACT) });
            }

            if (string.IsNullOrEmpty(USR))
            {
                yield return new ValidationResult("USR field is required!", new[] { nameof(USR) });
            }

            if (string.IsNullOrEmpty(WKS))
            {
                yield return new ValidationResult("WKS field is required!", new[] { nameof(WKS) });
            }
        }

    }

    public class MainParcel
    {
        public Parcel parcel { get; set; }
        public MatrixArticle[] matrixArticle { get; set; }
        public ParcelProperty[] parcelProperty { get; set; }
        public ParcelExplorer[] parcelExplorer { get; set; }
        public LegalFramework[] legalFramework { get; set; }
        public CASTAPARC[] CASTAPARC { get; set; }
    }


    public class LegalFrameworkList 
    {
        public LegalFramework legalFrameworks { get; set; }
        public Levantamento[] Levantamento { get; set; }
    }

    public class MainParcelByID
    {
        public Parcel parcel { get; set; }
        public MatrixArticle[] matrixArticle { get; set; }
        public ParcelProperty[] parcelProperty { get; set; }
        public ParcelExplorer[] parcelExplorer { get; set; }
        public LegalFrameworkList[] legalFramework { get; set; }
        public CASTAPARC[] CASTAPARC { get; set; }
        //public Levantamento[] Levantamento { get; set; }
    }

    public class ParcelName
    {
        public string DistrictName { get; set; }
        public string CountryName { get; set; }
        public string ParishName { get; set; }

        public string CLASSE { get; set; }

        public int NUMPARC { get; set; }
        public short VERSAO { get; set; }

        public byte IDSITLEG { get; set; }
        public string CODLITIGIO { get; set; }
        public string GEOCOD { get; set; }
        public decimal? NUMPARANT { get; set; }
        public string DESPARC { get; set; }
        public bool SITEDECLAR { get; set; }
        public bool? LEGALIZAVEL { get; set; }
        public decimal PONTUACAO { get; set; }
        public decimal? DTULTVIST { get; set; }
        public byte IDSITPARC { get; set; }
        public int? IDTPESTADOVINHA { get; set; }
        public decimal? ANOPLANTACAO { get; set; }
        public int CODDIS { get; set; }

        public int CODCON { get; set; }

        public int CODFRG { get; set; }
   //     public decimal? AREADISPDOURO { get; set; }


    }
 /*   [Table("t_dtplantacao", Schema = "s_dominio")]

    public class Plantacao
    {
        public decimal? AREADISPDOURO { get; set; }
     
        [Key]

        public decimal? IDDTPLANTACAO { get; set; }
    }
*/
    public class ParcelList
    {
        public ParcelName parcel { get; set; }

        public ParcelProperty[] parcelProperty { get; set; }
        public ParcelExplorer[] parcelExplorer { get; set; }
    }
}
