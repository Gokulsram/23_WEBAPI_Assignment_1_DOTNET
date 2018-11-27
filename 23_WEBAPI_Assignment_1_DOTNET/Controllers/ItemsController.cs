using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace _23_WEBAPI_Assignment_1_DOTNET.Controllers
{
    public class ItemsController : Controller
    {
        // GET: Items
        public ActionResult Index()
        {
            IEnumerable<ItemViewModel> items = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61883/api/");
                //HTTP GET
                var responseTask = client.GetAsync("item");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<ItemViewModel>>();
                    readTask.Wait();

                    items = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    items = Enumerable.Empty<ItemViewModel>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return View(items);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult create(ItemViewModel items)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61883/api/item");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<ItemViewModel>("item", items);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(items);
        }

        public ActionResult Edit(string itemcode)
        {
            ItemViewModel items = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61883/api/");
                //HTTP GET
                var responseTask = client.GetAsync("item?itemcode=" + itemcode.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ItemViewModel>();
                    readTask.Wait();

                    items = readTask.Result;
                }
            }

            return View(items);
        }

        [HttpPost]
        public ActionResult Edit(ItemViewModel items)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61883/api/item");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<ItemViewModel>("item", items);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(items);
        }

        public ActionResult Delete(string itemcode)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:61883/api/");

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("Item?itemCode=" + itemcode);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}