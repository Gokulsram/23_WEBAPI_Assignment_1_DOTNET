using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace _23_WEBAPI_Assignment_1_DOTNET
{
    public class POMasterDetailsController : Controller
    {
        // GET: POMasterDetails
        public ActionResult Index()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:61883/api/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("PODetail").Result;

            if (response.IsSuccessStatusCode)
                return View(response.Content.ReadAsAsync<IEnumerable<PODetailModel>>().Result);
            else
                return View();
        }
    }
}