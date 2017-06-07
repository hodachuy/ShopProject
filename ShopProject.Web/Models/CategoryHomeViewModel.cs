using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopProject.Web.Models
{
    public class CategoryHomeViewModel
    {
        public IEnumerable<ProductCategoryViewModel> MenuCategory { set; get; }
        public IEnumerable<GroupViewModel> Groups { set; get; }
    }
}