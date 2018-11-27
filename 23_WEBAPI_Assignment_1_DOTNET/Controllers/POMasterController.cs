using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _23_WEBAPI_Assignment_1_DOTNET.Controllers
{
    public class POMasterController : Controller
    {
       SelectList lstSupplier;
        public POMasterController()
        {
            POMasterClient CC = new POMasterClient();
            lstSupplier = new SelectList(CC.GetAllSupplier(), "SUPLNO", "SUPLNAME");
        }
        // GET: POMaster
        public ActionResult Index()
        {
            POMasterClient CC = new POMasterClient();
            return View(CC.GetAllPOMaster());
        }

        public ActionResult Create()
        {
            POMasterModel p = new POMasterModel();
            p.lstSupplier = lstSupplier;
            return View(p);
        }

        [HttpPost]
        public ActionResult Create(POMasterModel pomaster)
        {
            POMasterClient CC = new POMasterClient();
            CC.Create(pomaster);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            POMasterClient CC = new POMasterClient();
            POMasterModel CVM = new POMasterModel();
            CVM = CC.GetPOMasterById(id);
            CVM.lstSupplier = lstSupplier;
            return View("Edit", CVM);
        }
        [HttpPost]
        public ActionResult Edit(POMasterModel pOMasterModel)
        {
            POMasterClient CC = new POMasterClient();
            CC.Edit(pOMasterModel);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            POMasterClient CC = new POMasterClient();
            CC.Delete(id);
            return RedirectToAction("Index");
        }

    }
}