using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MeridixWebApiSignHelper.Models
{
    public class ResultModel
    {
        public string RequestUrl { get; set; }
        public string RequestQuery { get; set; }
        
        public string Token { get; set; }
        public string Secret { get; set; }
        public string ParametersConcated { get; set; }
        public string ParametersConcatedEncoded { get; set; }
        public string RequestEncoded { get; set; }
        public string VerbRequestQuery { get; set; }
        public string Signature { get; set; }
        public string SignedRequest { get; set; }
        public ParametersModel ParametersModel { get; set; }
        public string Verb { get; set; }
    }

    public class ParametersModel
    {
        //public string token = "35f94ba7c9bd4b8887b66baa8b566c28";
        //public string secret = "2c9e39f72f434a8";
        //public string request = "http://temporary.meridix.se/iCentrex36/SSO";
        //public string username = "basic@sigtunahem.se";

        public string Token { get; set; }
        public string Secret { get; set; }
        public string Nonce { get; set; }

        [Display(Name = "Request URL (with query string)")]
        public string RequestUrl { get; set; }

        [Display(Name = "Timestamp (UTC) - yyyymmddhhmmss")]
        public string Timestamp { get; set; }

        [Display(Name = "HTTP Verb - [GET|POST|PUT|DELETE]")]
        public string HttpVerb { get; set; }

        public ParametersModel()
        {
            Nonce = Guid.NewGuid().ToString().Substring(0, 8);
            Timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            HttpVerb = "GET";
        }
    }
}