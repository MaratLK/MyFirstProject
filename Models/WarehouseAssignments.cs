    using System.ComponentModel.DataAnnotations;

    namespace PLK_TwoTry_Back.Models
    {
        public class WarehouseAssignments
        {
            [Key]
            public int AssignmentID { get; set; }
            public int OrderID { get; set; }
            public int WarehouseID { get; set; }
            public DateTime AssignmentDate { get; set; }
            public Orders Order { get; set; }
            public Warehouses Warehouse { get; set; }
        }
    }
