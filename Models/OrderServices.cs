using PLK_TwoTry_Back.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class OrderServices
{
    [Key]
    public int OrderServiceID { get; set; }
    public int OrderID { get; set; }
    public int ServiceID { get; set; }
    public int Quantity { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [ForeignKey("OrderID")]
    public Orders Order { get; set; }

    [ForeignKey("ServiceID")]
    public Services Service { get; set; }
}
