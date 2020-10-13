using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace IVPD.Models
{
    public class Levantamento
    {
		[Key]
		public int ID { get; set; }
		public int? IDLEVANTAMENTO { get; set; }
		public decimal? CODIGO { get; set; }

		[Required]
		public string GEOCOD { get; set; }

		[Required]
		public decimal AREA { get; set; }

		[Required]
		public string NMPREDPLANT { get; set; }
		public int? REQ { get; set; }
		public short? ANO { get; set; }
		public decimal? NIF { get; set; }
		public string NMENT { get; set; }
		public int? NRVITIC { get; set; }
		public string NMPREDIO { get; set; }
		public int? INIDIREITO { get; set; }
		public int? FIMDIREITO { get; set; }
		public string TIPODIREITO { get; set; }
		public int? ANOPLANT { get; set; }
		public bool? TDR { get; set; }
		public bool? EXT { get; set; }
		public string CONCPLANT { get; set; }
		public string FRGPLANT { get; set; }
		public string CONCPARCORIG { get; set; }
		public string FRGPARCORIG { get; set; }
		public DateTime? DTENVIO { get; set; }

		[Required]
		public string STATUS { get; set; }
		public decimal? DTACT { get; set; }
		public decimal? HRACT { get; set; }

		[Required]
		public string USR { get; set; }

		[Required]
		public string WKS { get; set; }
		public int? NRLICENCA { get; set; }
		public string CODDIS { get; set; }
		public string CODCON { get; set; }
		public string CODFRG { get; set; }
		public decimal? IDTIPOLICENCA { get; set; }
		public byte? IDSITLEG { get; set; }
		public int? NRPES { get; set; }
		public decimal? DTREQ { get; set; }
		public decimal? DTEMISSAO { get; set; }
		public string? OBS { get; set; }
		public DateTime? DTINSERT { get; set; }
		public string USRACT { get; set; }
		public string WKSACT { get; set; }

		[Required]
		public bool LICENCA { get; set; }
		public decimal? AREAAPLICADA { get; set; }
		public int? IDCLASSEPLANT { get; set; }
	}

}
