using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _23_WEBAPI_Assignment_1_DOTNET.Controllers.Api
{
    public class POMasterController : ApiController
    {
        public POMasterController()
        {

        }

        public IHttpActionResult GetAllPOMaster()
        {
            IList<POMasterModel> master = new List<POMasterModel>();
            using (var ctx = new PODbEntities())
            {

                var resultt = (from d in ctx.POMASTERs
                               join f in ctx.SUPPLIERs
                               on d.SUPLNO equals f.SUPLNO
                               select new
                               {
                                   PONO = d.PONO,
                                   PODATE = d.PODATE,
                                   SUPLNO = d.SUPLNO,
                                   SUPLNAME = f.SUPLNAME
                               }).ToList();

                foreach (var item in resultt)
                {
                    var adPerson = new POMasterModel()
                    {
                        SUPLNO = item.SUPLNO.Trim(),
                        SUPLNAME = item.SUPLNAME.Trim(),
                        PONO = item.PONO.Trim(),
                        PODATE = item.PODATE
                    };
                    master.Add(adPerson);
                }
            }

            if (master.Count == 0)
                return NotFound();

            return Ok(master);
        }

        public IHttpActionResult PostNewPOMaster(POMasterModel pOMasterModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new PODbEntities())
            {
                pOMasterModel.SUPLNO = pOMasterModel.SelectedSupplier;

                ctx.POMASTERs.Add(new POMASTER()
                {
                    PONO = pOMasterModel.PONO.Trim(),
                    PODATE = pOMasterModel.PODATE,
                    SUPLNO = pOMasterModel.SUPLNO.Trim()
                });

                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult GetPOMasterByID(string id)
        {
            POMasterModel master = new POMasterModel();
            using (var ctx = new PODbEntities())
            {

                var resultt = (from d in ctx.POMASTERs
                               join f in ctx.SUPPLIERs
                               on d.SUPLNO equals f.SUPLNO
                               where d.PONO == id
                               select new
                               {
                                   PONO = d.PONO,
                                   PODATE = d.PODATE,
                                   SUPLNO = d.SUPLNO,
                                   SUPLNAME = f.SUPLNAME,
                                   SelectedSupplier = d.SUPLNO.Trim()
                               }).ToList();

                foreach (var item in resultt)
                {
                    master.SUPLNO = item.SUPLNO.Trim();
                    master.SUPLNAME = item.SUPLNAME.Trim();
                    master.PONO = item.PONO.Trim();
                    master.PODATE = item.PODATE;
                    master.SelectedSupplier = item.SelectedSupplier.Trim();
                    break;
                }
            }

            if (master == null)
                return NotFound();

            return Ok(master);
        }

        public IHttpActionResult Put(POMasterModel supplier)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new PODbEntities())
            {
                var existingItems = ctx.POMASTERs.Where(s => s.PONO == supplier.PONO)
                                                        .FirstOrDefault<POMASTER>();

                if (existingItems != null)
                {
                    existingItems.PODATE = supplier.PODATE;
                    existingItems.SUPLNO = supplier.SelectedSupplier.Trim();
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        public IHttpActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Not a valid item Code");

            using (var ctx = new PODbEntities())
            {
                var item = ctx.POMASTERs
                    .Where(s => s.PONO == id)
                    .FirstOrDefault();

                if (item != null)
                {
                    ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
            }

            return Ok();
        }

        [Route("api/Suppliers")]
        [HttpGet]
        public IHttpActionResult GetAllSupplier()
        {
            IList<SupplierViewModel> items = null;
            using (var ctx = new PODbEntities())
            {
                items = ctx.SUPPLIERs.Select(s => new SupplierViewModel()
                {
                    SUPLNO = s.SUPLNO,
                    SUPLNAME = s.SUPLNAME,
                }
                ).ToList<SupplierViewModel>();
            }
            if (items.Count == 0)
                return NotFound();

            return Ok(items);
        }

    }

}
