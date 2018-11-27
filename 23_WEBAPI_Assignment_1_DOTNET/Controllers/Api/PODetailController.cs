using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _23_WEBAPI_Assignment_1_DOTNET
{
    public class PODetailController : ApiController
    {

        public IHttpActionResult GetAllPOMasterDetails()
        {
            IList<PODetailModel> items = null;
            using (var ctx = new PODbEntities())
            {
                items = ctx.PODETAILs.Select(s => new PODetailModel()
                {
                    ITCODE = s.ITCODE,
                    PONO = s.PONO,
                    QTY = s.QTY
                }
                ).ToList<PODetailModel>();
            }
            if (items.Count == 0)
                return NotFound();

            return Ok(items);
        }

        [Route("api/POMasterList")]
        [HttpGet]
        public IHttpActionResult GetAllPOMaster()
        {
            IList<POMasterModel> items = null;
            using (var ctx = new PODbEntities())
            {
                items = ctx.POMASTERs.Select(s => new POMasterModel()
                {
                    PONO = s.PONO
                }
                ).ToList<POMasterModel>();
            }
            if (items.Count == 0)
                return NotFound();

            return Ok(items);
        }

        [Route("api/GetAllItemsList")]
        [HttpGet]
        public IHttpActionResult GetAllItms()
        {
            IList<ItemViewModel> items = null;
            using (var ctx = new PODbEntities())
            {
                items = ctx.ITEMs.Select(s => new ItemViewModel()
                {
                    ITCODE = s.ITCODE,
                    ITDESC = s.ITDESC
                }
                ).ToList<ItemViewModel>();
            }
            if (items.Count == 0)
                return NotFound();

            return Ok(items);
        }

        public IHttpActionResult PostNewPOMasterDetails(PODETAIL pOMasterModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new PODbEntities())
            {
                ctx.PODETAILs.Add(new PODETAIL()
                {
                    PONO = pOMasterModel.PONO.Trim(),
                    QTY = pOMasterModel.QTY,
                    ITCODE = pOMasterModel.ITCODE
                });

                ctx.SaveChanges();
            }

            return Ok();
        }
        [Route("api/POMasterEdit")]
        public IHttpActionResult Put(PODetailModel pODetailModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new PODbEntities())
            {
                var existingItems = ctx.PODETAILs.Where(s => s.PONO == pODetailModel.PONO && s.ITCODE==pODetailModel.ITCODE)
                    .FirstOrDefault<PODETAIL>();

                if (existingItems != null)
                {
                    existingItems.QTY = pODetailModel.QTY;
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        [Route("api/POMasterDelete/{id}/{itcode}")]
        public IHttpActionResult Delete(string id,string itcode)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("Not a valid item Code");

            using (var ctx = new PODbEntities())
            {
                var item = ctx.PODETAILs
                    .Where(s => s.PONO == id && s.ITCODE== itcode)
                    .FirstOrDefault();

                if (item != null)
                {
                    ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                }
            }

            return Ok();
        }

    }
}
