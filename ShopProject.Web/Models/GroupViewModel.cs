using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopProject.Web.Models
{
    public class GroupViewModel
    {
        public int ID { set; get; }

        [MaxLength(256)]
        [Required]
        public string Name { set; get; }
        public string Image { set; get; }
        public string Content { set; get; }
        public string Alias { set; get; }
        public bool Status { set; get; }
        public bool? MenuFlag { set; get; }
        public bool? LeftFlag { set; get; }
        public bool? HomeFlag { set; get; }
        public virtual IEnumerable<ProductGroupViewModel> ProductGroups { set; get; }
        public virtual IEnumerable<ProductViewModel> Products { set; get; }
    }
}