public class NewsDTO
{
    public string Title { get; set; }  // Заголовок новости
    public string Content { get; set; }  // Содержание новости
    public DateTime DatePublished { get; set; }  // Дата публикации

    public int UserID { get; set; }  // Идентификатор пользователя, который добавил новость

    // Изменяем на nullable для того, чтобы изображения были необязательными
    public List<IFormFile>? Images { get; set; }
}
