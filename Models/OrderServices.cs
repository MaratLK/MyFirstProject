using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PLK_TwoTry_Back.Models
{
    public class OrderServices
    {
        [Key]
        public int OrderServiceID { get; set; }
        public int OrderID { get; set; }
        public int ServiceID { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public Orders Order { get; set; }
        public Services Service { get; set; }
    }
}
