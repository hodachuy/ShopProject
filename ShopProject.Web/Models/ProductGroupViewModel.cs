using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopProject.Web.Models
{
    public class ProductGroupViewModel
    {
        public int ProductID { set; get; }

        public int GroupID { set; get; }

        public virtual ProductViewModel Product { set; get; }

        public virtual GroupViewModel Group { set; get; }
    }
}