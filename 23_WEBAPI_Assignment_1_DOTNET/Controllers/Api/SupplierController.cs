using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace _23_WEBAPI_Assignment_1_DOTNET.Controllers.Api
{
    public class SupplierController : ApiController
    {
        public IHttpActionResult GetAllSupplier()
        {
            IList<SupplierViewModel> items = null;
            using (var ctx = new PODbEntities())
            {
                items = ctx.SUPPLIERs.Select(s => new SupplierViewModel()
                {
                    SUPLNO = s.SUPLNO,
                    SUPLNAME = s.SUPLNAME,
                    SUPLADDR = s.SUPLADDR
                }
                ).ToList<SupplierViewModel>();
            }
            if (items.Count == 0)
                return NotFound();

            return Ok(items);
        }

        public IHttpActionResult GetSupplierByNo(string SupplierNo)
        {
            IList<SupplierViewModel> items = null;

            using (var ctx = new PODbEntities())
            {
                items = ctx.SUPPLIERs
                    .Where(s => s.SUPLNO.ToLower() == SupplierNo.ToLower())
                    .Select(s => new SupplierViewModel()
                    {
                        SUPLNO = s.SUPLNO,
                        SUPLNAME = s.SUPLNAME,
                        SUPLADDR = s.SUPLADDR

                    }).ToList<SupplierViewModel>();
            }

            if (items.Count == 0)
            {
                return NotFound();
            }

            return Ok(items[0]);
        }

        public IHttpActionResult PostNewItem(SupplierViewModel supplier)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new PODbEntities())
            {
                ctx.SUPPLIERs.Add(new SUPPLIER()
                {
                    SUPLNO = supplier.SUPLNO,
                    SUPLNAME = supplier.SUPLNAME,
                    SUPLADDR = supplier.SUPLADDR
                });

                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult Put(SupplierViewModel supplier)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new PODbEntities())
            {
                var existingItems = ctx.SUPPLIERs.Where(s => s.SUPLNO == supplier.SUPLNO)
                                                        .FirstOrDefault<SUPPLIER>();

                if (existingItems != null)
                {
                    existingItems.SUPLNAME = supplier.SUPLNAME;
                    existingItems.SUPLADDR = supplier.SUPLADDR;
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

   
        public IHttpActionResult DeleteSupplier(string supplierNo)
        {
            if (string.IsNullOrEmpty(supplierNo))
                return BadRequest("Not a valid Supplier No");

            using (var ctx = new PODbEntities())
            {
                var item = ctx.SUPPLIERs
                    .Where(s => s.SUPLNO == supplierNo)
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
