using System.ComponentModel.DataAnnotations;

namespace PLK_TwoTry_Back.Models
{
    public class VehicleAssignments
    {
        [Key]
        public int AssignmentID { get; set; }
        public int OrderID { get; set; }
        public int VehicleID { get; set; }
        public DateTime AssignmentDate { get; set; }
        public Orders Order { get; set; }
        public SpecialVehicles Vehicle { get; set; }
    }
}
