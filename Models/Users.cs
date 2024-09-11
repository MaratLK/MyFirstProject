using PLK_TwoTry_Back.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Users
{
    [Key]
    public int UserID { get; set; }

    [Required]
    public int RoleID { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]  // Сделать обязательным
    public string CompanyName { get; set; }

    public string? Address { get; set; }  // Необязательное поле

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime DateRegistered { get; set; }

    [ForeignKey("RoleID")]
    public Roles Role { get; set; }
}
