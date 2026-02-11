using Aplicacao.DTO;
using Aplicacao.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EncurtadorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShortenController(IShortService shortService) : ControllerBase
    {
        private readonly IShortService _shortService = shortService;

        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] UrlDTO urlDto)
        {
            try
            {
                var result = await _shortService.UrlCode(urlDto);
                return Ok(new 
                { 
                    success = true,
                    shortcode = result.Shortcode,
                    longUrl = result.LongUrl,
                    shortUrl = $"{Request.Scheme}://{Request.Host}/api/shorten/{result.Shortcode}"
                });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { success = false, error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        [HttpGet("{shortcode}")]
        public async Task<IActionResult> GetLongUrl(string shortcode)
        {
            try
            {
                var longUrl = await _shortService.GetLongUrl(shortcode);

                // Redireciona para a URL original (comportamento padrão de URL shortener)
                return Redirect(longUrl);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { success = false, error = $"Shortcode '{shortcode}' not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        [HttpGet("stats/{shortcode}")]
        public async Task<IActionResult> GetStats(string shortcode)
        {
            try
            {
                var longUrl = await _shortService.GetLongUrl(shortcode);
                return Ok(new 
                { 
                    success = true, 
                    shortcode = shortcode,
                    longUrl = longUrl,
                    createdAt = DateTime.UtcNow // Em produção, armazene isso
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { success = false, error = $"Shortcode '{shortcode}' not found" });
            }
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "OK", message = "API de Encurtamento de URL está funcionando!" });
        }
    }
}
