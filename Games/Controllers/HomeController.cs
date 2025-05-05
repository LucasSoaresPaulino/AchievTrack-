using Games.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ProjectGames.BLL;

namespace Games.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        
    }
}