using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MeridixWebApiSignHelper.Models;

namespace MeridixWebApiSignHelper.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var model = new ParametersModel();
            return View(model);
        }

        public ActionResult Result(ParametersModel parametersModel)
        {
            var model = new ResultModel();
            model.ParametersModel = parametersModel;
            model.RequestUrl = parametersModel.RequestUrl;
            model.Token = parametersModel.Token;
            model.Secret = parametersModel.Secret;
            model.Verb = parametersModel.HttpVerb.ToUpperInvariant();

            var uri = new Uri(model.RequestUrl);
            model.RequestUrl = uri.GetLeftPart(UriPartial.Path);
            model.RequestQuery = uri.Query;

            var query = uri.Query;
            var queryParameters = HttpUtility.ParseQueryString(query);
            var parameters = new List<string>();

            foreach (string key in queryParameters.Keys)
                parameters.Add(key + "=" + queryParameters[key]);

            parameters.Add("auth_nonce=" + parametersModel.Nonce);
            parameters.Add("auth_timestamp=" + parametersModel.Timestamp);
            parameters.Add("auth_token=" + parametersModel.Token);
            parameters.Sort();

            model.ParametersConcated = string.Join("&", parameters);
            model.ParametersConcatedEncoded = Uri.EscapeDataString(model.ParametersConcated);
            model.RequestEncoded = Uri.EscapeDataString(model.RequestUrl);

            model.VerbRequestQuery = model.Verb + "&" + model.RequestEncoded + "&" + model.ParametersConcatedEncoded + "&" + model.Secret;

            byte[] bytes = Encoding.UTF8.GetBytes(model.VerbRequestQuery);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] signatureBytes = md5.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < signatureBytes.Length; i++)
                stringBuilder.Append(signatureBytes[i].ToString("x2"));

            model.Signature = stringBuilder.ToString();

            model.SignedRequest = model.RequestUrl + "?" + model.ParametersConcated + "&auth_signature=" + stringBuilder.ToString();

            return View(model);
        }

    }
}
