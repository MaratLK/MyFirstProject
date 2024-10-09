using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLKTransit.Data;
using PLK_TwoTry_Back.Models;
using System.Security.Claims;

namespace PLK_TwoTry_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly PLKTransitContext _context;

        public NewsController(PLKTransitContext context)
        {
            _context = context;
        }

        // Получить все новости
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            var newsList = await _context.News.Include(n => n.NewsImages).ToListAsync();
            return Ok(newsList);
        }

        // Получить новость по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<News>> GetNews(int id)
        {
            var news = await _context.News.Include(n => n.NewsImages).FirstOrDefaultAsync(n => n.NewsID == id);

            if (news == null)
            {
                return NotFound();
            }

            return Ok(news);
        }

        // Добавить новость (только для админов) с использованием DTO
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<News>> AddNews([FromForm] NewsDTO newsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  // Получаем ID текущего пользователя
            if (!int.TryParse(userId, out int parsedUserId))
            {
                return Unauthorized("Пользователь не авторизован");
            }

            // Создаем новость на основе данных из DTO
            var news = new News
            {
                Title = newsDto.Title,
                Content = newsDto.Content,
                DatePublished = newsDto.DatePublished,
                UserID = parsedUserId
            };

            // Сохраняем новость в базе данных
            _context.News.Add(news);
            await _context.SaveChangesAsync();

            // Обрабатываем изображения
            if (newsDto.Images != null && newsDto.Images.Count > 0)
            {
                foreach (var image in newsDto.Images)
                {
                    var imageUrl = await SaveImageToServer(image);  // Сохраняем файл на сервере
                    var newsImage = new NewsImage
                    {
                        ImageUrl = imageUrl,
                        NewsID = news.NewsID
                    };

                    _context.NewsImages.Add(newsImage);
                }

                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetNews), new { id = news.NewsID }, news);
        }

        // Обновить новость (только для админов)
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNews(int id, [FromForm] NewsDTO newsDto)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            news.Title = newsDto.Title;
            news.Content = newsDto.Content;
            news.DatePublished = newsDto.DatePublished;

            _context.Entry(news).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Удалить новость (только для админов)
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.Include(n => n.NewsImages).FirstOrDefaultAsync(n => n.NewsID == id);
            if (news == null)
            {
                return NotFound();
            }

            // Удаление изображений из файловой системы
            foreach (var image in news.NewsImages)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", image.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.NewsID == id);
        }

        // Вспомогательный метод для сохранения изображений на сервере
        private async Task<string> SaveImageToServer(IFormFile image)
        {
            if (image.Length > 5 * 1024 * 1024) // Ограничение на 5MB
            {
                throw new InvalidOperationException("Размер файла слишком большой.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException("Неподдерживаемый формат изображения.");
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/uploads/" + uniqueFileName;
        }
    }
}
