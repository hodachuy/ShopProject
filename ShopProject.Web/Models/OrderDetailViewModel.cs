using System;

namespace ShopProject.Web.Models
{
    public class OrderDetailViewModel
    {
        public int OrderID { set; get; }

        public int ProductID { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public int Amount
        {
            get
            {
                return Decimal.ToInt32(Quantity * Price);
            }
        }

        public virtual OrderViewModel Order { set; get; }

        public virtual ProductViewModel Product { set; get; }


    }
}