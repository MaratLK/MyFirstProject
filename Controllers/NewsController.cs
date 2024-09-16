using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLK_TwoTry_Back.Models;
using PLKTransit.Data;
using System.IO;

namespace PLKTransit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly PLKTransitContext _context;
        private readonly IWebHostEnvironment _env; // Для работы с файлами в wwwroot

        public NewsController(PLKTransitContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;  // Инициализируем _env для использования в сохранении файлов
        }

        // POST: api/News
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<News>> PostNews([FromForm] News news, [FromForm] int userID, [FromForm] List<IFormFile> images = null)
        {
            // Найти пользователя по userID
            var currentUser = await _context.Users.FindAsync(userID);
            if (currentUser == null)
            {
                return BadRequest("Пользователь не найден.");
            }

            // Привязка пользователя к новости
            news.User = currentUser;
            news.DatePublished = DateTime.Now;

            // Добавление новости в базу данных
            _context.News.Add(news);

            // Обработка изображений
            if (images != null && images.Count > 0)
            {
                var uploadPath = Path.Combine(_env.WebRootPath, "images/news");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                foreach (var image in images)
                {
                    var filePath = Path.Combine(uploadPath, image.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Сохранение изображения в базу данных
                    var newsImage = new NewsImage
                    {
                        ImageUrl = $"/images/news/{image.FileName}",
                        News = news
                    };
                    _context.NewsImages.Add(newsImage);
                }
            }

            // Сохранение изменений в базе данных
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNews", new { id = news.NewsID }, news);
        }


        // Пример метода получения всех новостей
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            var newsList = await _context.News
                                         .Include(n => n.NewsImages)  // Включаем изображения в ответ
                                         .ToListAsync();

            return Ok(newsList);
        }
    }
}
