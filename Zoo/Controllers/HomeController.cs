using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Zoo.Models;
using ZooDAL.Entities;
using ZooDAL.Services.Intefaces;

namespace Zoo.Controllers
{
    public class HomeController : Controller
    {
        readonly ICategoryService categoryService;

        public HomeController(ICategoryService service)
        {
            categoryService = service;
        }

        public IActionResult Index()
        {
            var test = new Category { Name = "Eli" };
            categoryService.CreateAsync(test).Wait();
            return View();
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