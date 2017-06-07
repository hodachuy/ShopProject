using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopProject.Model.Models
{
    [Table("Groups")]
    public class Group
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        [MaxLength(256)]
        [Required]
        public string Name { set; get; }
        public string Alias { set; get; }
        public string Image { set; get; }
        public string Content { set; get; }
        public bool Status { set; get; }
        public bool? MenuFlag { set; get; }
        public bool? LeftFlag { set; get; }
        public bool? HomeFlag { set; get; }
        public virtual IEnumerable<ProductGroup> ProductGroups { set; get; }

    }
}
