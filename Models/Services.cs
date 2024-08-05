using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLK_TwoTry_Back.Models
{
    public class Services
    {
        [Key]
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
