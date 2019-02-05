using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NeonTest.Models;

namespace NeonTest.Controllers
{
    public class HomeController : Controller
    {
        private const string base_url = "http://www.apilayer.net/api/live?access_key=f2a4deb9fa7acfe8e57bc92e48ccaa77";

        //private static readonly HttpClient client = new HttpClient();

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Test()
        {
            var responseContent = new object();
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(base_url);
                string responseString = String.Empty;

                if (response.IsSuccessStatusCode)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                    responseContent = Newtonsoft.Json.JsonConvert.DeserializeObject(responseString);
                }
            }

            return Json(responseContent);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
