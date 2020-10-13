using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace IVPD.Helpers
{
    public class APIRequest : FilterClass
    {
        public string modulename { get; set; }
        public long id { get; set; }


    }

    public class APIAuditRequest
    {
        public string comment { get; set; }
    }


    public class APIParcelRequest
    {
        public int id { get; set; }
        public short versao { get; set; }
    }
    public class APIConsultCurrentAccountRequest
    {
        public long txtNUMENT { get; set; }
        public string dateInicial { get; set; }
        public string dateFinal { get; set; }

    }
    public class APIBilingAddressRequest
    {
        public int entity_id { get; set; }
    }
  

    public class APIInsertUpdateAmountRequest
    {
        public string caixa { get; set; }
        public string date { get; set; }
        public int closed { get; set; }

        public decimal cheques { get; set; }
        public decimal numerario { get; set; }
        public decimal opening_amount { get; set; }

    }

    public class APIGetAmountRequest
    {
        public string caixa { get; set; }
        public string date { get; set; }
      
    }
    public class APIPreviousBalanceRequest
    {
        public string date { get; set; }
        public Dictionary<string, object> Filters { get; set; }
        public string SortBy { get; set; }
        public bool? IsSortTypeDESC { get; set; }
        public bool? IsPagination { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }

    }
    public class APIGetCashValuesRequest
    {
        public string date { get; set; }
        public int transMethodId { get; set; }

        public Dictionary<string, object> Filters { get; set; }
        public string SortBy { get; set; }
        public bool? IsSortTypeDESC { get; set; }
        public bool? IsPagination { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }

    }

}
