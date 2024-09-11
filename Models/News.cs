using PLK_TwoTry_Back.Models;

public class News
{
    public int NewsID { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DatePublished { get; set; }
    public ICollection<NewsImage> NewsImages { get; set; }
    public Users User { get; set; }
}
