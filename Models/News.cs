using PLK_TwoTry_Back.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class News
{
    public int NewsID { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DatePublished { get; set; }
    public int UserID { get; set; }
    public List<NewsImage> NewsImages { get; set; } // Связь с изображениями
}