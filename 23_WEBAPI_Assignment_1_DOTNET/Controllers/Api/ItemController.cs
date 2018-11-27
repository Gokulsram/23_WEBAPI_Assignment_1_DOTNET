using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _23_WEBAPI_Assignment_1_DOTNET
{
    public class ItemController : ApiController
    {
        public ItemController()
        {

        }

        public IHttpActionResult GetAllItems()
        {
            IList<ItemViewModel> items = null;
            using (var ctx = new PODbEntities())
            {
                items = ctx.ITEMs.Select(s => new ItemViewModel()
                {
                    ITCODE = s.ITCODE,
                    ITRATE = s.ITRATE,
                    ITDESC = s.ITDESC
                }
                ).ToList<ItemViewModel>();
            }
            if (items.Count == 0)
                return NotFound();

            return Ok(items);
        }

        public IHttpActionResult GetAllItems(string Itemcode)
        {
            IList<ItemViewModel> items = null;

            using (var ctx = new PODbEntities())
            {
                items = ctx.ITEMs
                    .Where(s => s.ITCODE.ToLower() == Itemcode.ToLower())
                    .Select(s => new ItemViewModel()
                    {
                        ITCODE = s.ITCODE,
                        ITDESC = s.ITDESC,
                        ITRATE = s.ITRATE,
                      
                    }).ToList<ItemViewModel>();
            }

            if (items.Count == 0)
            {
                return NotFound();
            }

            return Ok(items[0]);
        }

        //public IHttpActionResult GetAllItems(string Itemcode)
        //{
        //    IList<ItemViewModel> items = null;
        //    using (var ctx = new PODbEntities())
        //    {
        //        items = ctx.ITEMs.Select(s => new ItemViewModel()
        //        {
        //            ITCODE = s.ITCODE,
        //            ITRATE = s.ITRATE,
        //            ITDESC = s.ITDESC
        //        }
        //        ).Where(s=>s.ITCODE==Itemcode).ToList<ItemViewModel>();
        //    }
        //    if (items.Count == 0)
        //        return NotFound();

        //    return Ok(items);
        //}

        public IHttpActionResult PostNewItem(ItemViewModel item)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new PODbEntities())
            {
                ctx.ITEMs.Add(new ITEM()
                {
                    ITCODE = item.ITCODE,
                    ITDESC = item.ITDESC,
                    ITRATE = item.ITRATE
                });

                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult Put(ItemViewModel item)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new PODbEntities())
            {
                var existingItems = ctx.ITEMs.Where(s => s.ITCODE == item.ITCODE)
                                                        .FirstOrDefault<ITEM>();

                if (existingItems != null)
                {
                    existingItems.ITDESC = item.ITDESC;
                    existingItems.ITRATE = item.ITRATE;
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        public IHttpActionResult Delete(string itemCode)
        {
            if (string.IsNullOrEmpty(itemCode))
                return BadRequest("Not a valid item Code");

            using (var ctx = new PODbEntities())
            {
                var item = ctx.ITEMs
                    .Where(s => s.ITCODE == itemCode)
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
