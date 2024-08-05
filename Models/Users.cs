using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace PLK_TwoTry_Back.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }
        public int RoleID { get; set; } = 3;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateRegistered { get; set; }
        public Roles? Role { get; set; }
    }
}
