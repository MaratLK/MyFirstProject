using PLK_TwoTry_Back.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class VehicleAssignments
{
    [Key]
    public int AssignmentID { get; set; }
    public int OrderID { get; set; }
    public int VehicleID { get; set; }
    public DateTime AssignmentDate { get; set; }

    [ForeignKey("OrderID")]
    public Orders Order { get; set; }

    [ForeignKey("VehicleID")]
    public SpecialVehicles Vehicle { get; set; }
}
