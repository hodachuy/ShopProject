using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopProject.Web.Models
{
    public class FileImageViewModel
    {
        public int ID { set; get; }

        [Column(TypeName = "ntext")]
        public string FileName { set; get; }
        public string Extension { set; get; }
        public int ProductImageID { set; get; }

    }
}