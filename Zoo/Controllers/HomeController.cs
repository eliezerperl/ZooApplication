using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Zoo.Models;
using ZooDAL.Entities;
using ZooDAL.Services.Intefaces;

namespace Zoo.Controllers
{
    public class HomeController : Controller
    {
        readonly ICategoryService _categoryService;
        readonly ICommentService _commentService;
        readonly IAnimalService _animalService;

        public HomeController(IAnimalService service, ICommentService cservice, ICategoryService catservice)
        {
            _animalService = service;
            _commentService = cservice;
            _categoryService = catservice;
        }

        public IActionResult Index()
        {
            var animals = _animalService.GetAllAnimalsWithCategories().Result;

            return View(animals);
        }

        public IActionResult Details(Guid id)
        {
            var animal = _animalService.GetAnimalWithCategory(id).Result;
            animal.Comments = _commentService.GetCommentsForAnimal(animal).Result;
            return View(animal);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Privacy(Animal animal)
        {
            _animalService.CreateAsync(animal);

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}