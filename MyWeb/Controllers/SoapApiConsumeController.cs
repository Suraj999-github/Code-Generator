using MyWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace MyWeb.Controllers
{
    public class SoapApiConsumeController : Controller
    {
        // GET: SoapApiConsume
        public ActionResult Index()
        {
            string result = null;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                string ScNo = "310.04.078B";
                string ConsumerId = "622066";
                string NEACounter = "216";
                string UserName = "Eprabhu";
                string Password = "NE@EPr@3Hu9874@$Ep";
                string AccessCode = "EP01";
                string Processid = DateTime.UtcNow.ToString();
                //Calling CreateSOAPWebRequest method

                string uri = "https://neabill.prabhubank.com.np/NEAWS_LIVE/webService.asmx";
                string soap = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:web='WebServices'>"
           + "<soapenv:Header/>"
           + "<soapenv:Body>"
               + "<web:GetCustomerBillInfo>"
                       + "<web:SCNO>" + ScNo + "</web:SCNO>"
                       + "<web:CONSUMER_ID>" + ConsumerId + "</web:CONSUMER_ID>"
                       + "<web:OFFICE_CODE>" + NEACounter + "</web:OFFICE_CODE>"
                       + "<web:USER_ID>" + UserName + "</web:USER_ID>"
                       + "<web:PASSWORD>" + Password + "</web:PASSWORD>"
                       + "<web:ACCESS_CODE>" + AccessCode + "</web:ACCESS_CODE>"
                       + "<web:PROCESSID>" + Processid + "</web:PROCESSID>"
               + "</web:GetCustomerBillInfo>"
           + "</soapenv:Body>"
           + "</soapenv:Envelope>";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Headers.Add("SOAPAction", "WebServices/GetCustomerBillInfo");
                request.ContentType = "text/xml;charset=\"utf-8\"";
                request.Accept = "text/xml";
                request.Method = "POST";

                using (Stream stm = request.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(stm))
                    {
                        stmw.Write(soap);
                    }
                }

                using (WebResponse webResponse = request.GetResponse())
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
                      using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                    {                       
                        var ServiceResult = rd.ReadToEnd();
                        result = ServiceResult;
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(result);
                        var jsonval = JsonConvert.SerializeXmlNode(doc);                      
                        var json = JsonConvert.DeserializeObject<Root>(jsonval);
                        var response = json.SoapEnvelope.SoapBody.GetCustomerBillInfoResponse.GetCustomerBillInfoResult;
                        return Json(response);                    
                    }
                }            
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return Json(msg);
            }            
        }
      
    }
}
