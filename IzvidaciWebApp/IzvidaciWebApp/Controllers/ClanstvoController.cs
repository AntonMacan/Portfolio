using IzvidaciWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IzvidaciWebApp.Controllers
{
    public class ClanstvoController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return Error();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}