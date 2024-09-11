using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PLK_TwoTry_Back.Models;
using PLKTransit.Data;

namespace PLK_TwoTry_Back.Controllers
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
            _env = env;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<News>>> GetNews()
        {
            var newsList = await _context.News.Include(n => n.NewsImages).ToListAsync();
            if (newsList == null || !newsList.Any())
            {
                return NotFound("Новости не найдены.");
            }
            return Ok(newsList);
        }

        // POST: api/News
        [HttpPost]
        [Authorize(Roles = "Admin")]  // Ограничиваем доступ только для админов
        [Consumes("multipart/form-data")] // Добавляем, чтобы Swagger знал о типе контента
        public async Task<ActionResult<News>> PostNews([FromForm] News news, [FromForm] List<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            news.DatePublished = DateTime.Now;
            _context.News.Add(news);

            // Проверка наличия директории для изображений и её создание при необходимости
            var directory = Path.Combine(_env.WebRootPath, "images/news");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Сохранение изображений
            if (images != null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    var filePath = Path.Combine(directory, image.FileName);

                    try
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"Ошибка при сохранении файла: {ex.Message}");
                    }

                    var newsImage = new NewsImage { ImageUrl = $"/images/news/{image.FileName}", News = news };
                    _context.NewsImages.Add(newsImage);
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNews", new { id = news.NewsID }, news);
        }

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]  // Ограничиваем доступ только для админов
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News
                                     .Include(n => n.NewsImages)
                                     .FirstOrDefaultAsync(n => n.NewsID == id);

            if (news == null)
            {
                return NotFound($"Новость с ID {id} не найдена.");
            }

            // Удаление изображений с сервера
            foreach (var image in news.NewsImages)
            {
                var filePath = Path.Combine(_env.WebRootPath, image.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        System.IO.File.Delete(filePath);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"Ошибка при удалении файла: {ex.Message}");
                    }
                }
            }

            // Удаление новости
            _context.News.Remove(news);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при удалении новости: {ex.Message}");
            }

            return NoContent();
        }
    }
}
