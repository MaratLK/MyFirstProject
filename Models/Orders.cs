using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Orders
{
    [Key]
    public int OrderID { get; set; }
    public int UserID { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public DateTime? DeliveryDate { get; set; }
    public string Status { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    [ForeignKey("UserID")]
    public Users User { get; set; }  // Навигационная проперти
}
