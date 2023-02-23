using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Soindus.Svc.Base64DTE.DBNet
{
    public class putCustomerETDLoadResponse
    {
        [JsonProperty("root")]
        public Root Root { get; set; }
    }

    public class Root
    {
        [JsonProperty("putCustomerETDLoadResult")]
        public putCustomerETDLoadResult putCustomerETDLoadResult { get; set; }
    }

    public class putCustomerETDLoadResult
    {
        [JsonProperty("Codigo")]
        public string Codigo { get; set; }
        [JsonProperty("Mensajes")]
        public string Mensajes { get; set; }
        [JsonProperty("TrackId")]
        public string TrackId { get; set; }
        [JsonProperty("Folio")]
        public string Folio { get; set; }
        [JsonProperty("FolioERP")]
        public string FolioERP { get; set; }
    }

    public class get_pdf_sucursalResponse
    {
        [JsonProperty("root")]
        public RootPDF Root { get; set; }
    }

    public class RootPDF
    {
        [JsonProperty("get_pdf_sucursalResult")]
        public GetPdfSucursalResult GetPdfSucursalResult { get; set; }
    }

    public partial class GetPdfSucursalResult
    {
        [JsonProperty("string")]
        public string[] String { get; set; }
    }
}
