using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

namespace _23_WEBAPI_Assignment_1_DOTNET
{
    public class POMasterModel
    {
        [DisplayName("PO Number")]
        public string PONO { get; set; }
        [DisplayName("PO Date")]
        public DateTime? PODATE { get; set; }
        [DisplayName("Supplier Number")]
        public string SUPLNO { get; set; }
        [DisplayName("Supplier Name")]
        public string SUPLNAME { get; set; }
        public SelectList lstSupplier { get; set; }
        public string SelectedSupplier { get; set; }

    }

}