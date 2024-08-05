using System.ComponentModel.DataAnnotations;

namespace PLK_TwoTry_Back.Models
{
    public class SpecialVehicles
    {
        [Key]
        public int VehicleID { get; set; }
        public string VehicleType { get; set; }
        public string LicensePlate { get; set; }
        public int Capacity { get; set; }
    }
}
