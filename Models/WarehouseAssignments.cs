using PLK_TwoTry_Back.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class WarehouseAssignments
{
    [Key]
    public int AssignmentID { get; set; }
    public int OrderID { get; set; }
    public int WarehouseID { get; set; }
    public DateTime AssignmentDate { get; set; }

    [ForeignKey("OrderID")]
    public Orders Order { get; set; }

    [ForeignKey("WarehouseID")]
    public Warehouses Warehouse { get; set; }
}
