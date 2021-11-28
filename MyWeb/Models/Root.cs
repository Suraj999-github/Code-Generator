using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class Root
    {
        [JsonProperty("?xml")]
        public Xml Xml { get; set; }

        [JsonProperty("soap:Envelope")]
        public SoapEnvelope SoapEnvelope { get; set; }
    }
    public class RETURNBILLINFONATIVE
    {
        public string CODE { get; set; }
        public string MESSAGE { get; set; }
    }

    public class TRANDETAILS
    {
        public RETURNBILLINFONATIVE RETURN_BILLINFO_NATIVE { get; set; }
    }

    public class GetCustomerBillInfoResult
    {
        public TRANDETAILS TRANDETAILS { get; set; }
    }

    public class GetCustomerBillInfoResponse
    {
        [JsonProperty("@xmlns")]
        public string Xmlns { get; set; }
        public GetCustomerBillInfoResult GetCustomerBillInfoResult { get; set; }
    }

    public class SoapBody
    {
        public GetCustomerBillInfoResponse GetCustomerBillInfoResponse { get; set; }
    }

    public class SoapEnvelope
    {
        [JsonProperty("@xmlns:soap")]
        public string XmlnsSoap { get; set; }

        [JsonProperty("@xmlns:xsi")]
        public string XmlnsXsi { get; set; }

        [JsonProperty("@xmlns:xsd")]
        public string XmlnsXsd { get; set; }

        [JsonProperty("soap:Body")]
        public SoapBody SoapBody { get; set; }
    }
    public class Xml
    {
        [JsonProperty("@version")]
        public string Version { get; set; }

        [JsonProperty("@encoding")]
        public string Encoding { get; set; }
    }
}