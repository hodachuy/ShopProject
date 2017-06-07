using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopProject.Model.Models
{
    [Table("FileImages")]
    public class FileImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }
        public string FileName { set; get; }
        public string Extension { set; get; }
        public int ProductImageID { set; get; }

        [ForeignKey("ProductImageID")]
        public virtual Product Product { set; get; }
    }
}
