using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Malicious_Middleware_Hacker_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void Post(string content)
        {
            var path = $"Logs\\{Guid.NewGuid().ToString()}.txt";
            using var sw = System.IO.File.CreateText(path);
            sw.WriteLine($"Pwned content received!: {content}");
        }
    }
}