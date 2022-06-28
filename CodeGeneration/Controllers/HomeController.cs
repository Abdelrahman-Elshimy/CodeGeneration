using CodeGeneration.Data;
using CodeGeneration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGeneration.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var serciesCounter = _context.Services.ToList().Count();
            var codesCounter = _context.Codes.ToList().Count();
            var codeinday = _context.Codes.Where(x => x.Date_Created.Year == DateTime.Now.Year && x.Date_Created.Month == DateTime.Now.Month && x.Date_Created.Day == DateTime.Now.Day).ToList().Count();
            var codeinmonth = _context.Codes.Where(x => x.Date_Created.Year == DateTime.Now.Year && x.Date_Created.Month == DateTime.Now.Month).ToList().Count();

            var stats = new Stats
            {
                CodesCounter = codesCounter,
                ServicesCounter = serciesCounter,
                CodesPerDay = codeinday,
                CodesPerMonth = codeinmonth
            };
            return View(stats);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
