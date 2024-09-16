using PLK_TwoTry_Back.Models;
using System.ComponentModel.DataAnnotations;

public class News
{
    public int NewsID { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DatePublished { get; set; }

    [Required]  // Возможно, здесь требуется добавить [Required] для userID, если оно обязательно
    public int UserID { get; set; }

    public Users? User { get; set; }  // Навигационное свойство для связи с таблицей Users

    public ICollection<NewsImage> NewsImages { get; set; } = new List<NewsImage>();
}
