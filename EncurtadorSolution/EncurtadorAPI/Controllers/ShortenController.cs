using Aplicacao.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EncurtadorAPI.Controllers
{
    public class ShortenController(IShortService shortService) : ControllerBase
    {
        private readonly IShortService _shortService = shortService;

        public IActionResult Index()
        {
            return  Task.IsCompleted;
        }
    }
}
