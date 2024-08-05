using System.ComponentModel.DataAnnotations;

namespace PLK_TwoTry_Back.Models
{
    public class Warehouses
    {
        [Key]
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
    }
}
