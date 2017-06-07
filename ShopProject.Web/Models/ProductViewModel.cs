using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopProject.Web.Models
{
    [Serializable]
    public class ProductViewModel
    {
        public int ID { set; get; }

        [Required]
        public string Name { set; get; }

        [Required]
        public string Alias { set; get; }

        public int CategoryID { set; get; }

        public string Image { set; get; }

        public decimal Price { set; get; }

        public decimal? PromotionPrice { set; get; }

        public string Warranty { set; get; }

        public string Description { set; get; }

        public string Content { set; get; }

        public bool? HomeFlag { set; get; }

        public bool? HotFlag { set; get; }
        public bool? ProductNewFlag { set; get; }
        public bool? DiscountFlag { set; get; }

        public int? ViewCount { set; get; }

        public DateTime? CreatedDate { set; get; }

        public string CreatedBy { set; get; }

        public DateTime? UpdatedDate { set; get; }

        public string UpdatedBy { set; get; }

        public string MetaKeyword { set; get; }

        public string MetaDescription { set; get; }

        public bool Status { set; get; }

        public string[] Tags { set; get;}

        public IEnumerable<TagViewModel> TagsVm { set; get; }


        public int Quantity { set; get; }

        public decimal OriginalPrice { set; get; }

        public string[] MoreImages { set; get; }
        public virtual ProductCategoryViewModel ProductCategory { set; get; }
        public virtual IEnumerable<FileImageViewModel> FileImages { set; get; }
        public virtual IEnumerable<GroupViewModel> Groups { set; get; }
    }
}