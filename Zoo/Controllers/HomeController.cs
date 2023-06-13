using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var animals = _animalService.GetTopTwoAnimalsWithCategories().Result;

            return View(animals);
        }

        public IActionResult Details(Guid Id)
        {
            var animal = _animalService.GetAnimalWithCategory(Id).Result;
            animal.Comments = _commentService.GetCommentsForAnimal(animal).Result;
            return View(animal);
        }

        [HttpPost]
        public IActionResult PostComment(string comment, Guid Id)
        {
            var userComment = new Comment
            {
                Content = comment,
                AnimalID = Id,
            };
            _commentService.CreateAsync(userComment);
            return RedirectToAction(nameof(Details), new {Id});
        }

        public IActionResult Catalog(Guid? categoryId)
        {
            IEnumerable<Animal> animals;
            if (categoryId.HasValue && categoryId != Guid.Empty) 
                animals = _animalService.GetAnimalsByCategory(categoryId.Value).Result;
            else
                animals = _animalService.GetAllAnimalsWithCategories().Result;

            var categories = _categoryService.GetAllAsync().Result;
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);

            return View(animals);
        }


        [HttpPost]
        public IActionResult GetByCategory(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                return RedirectToAction(nameof(Catalog));
            else
                return RedirectToAction(nameof(Catalog), new { categoryId });
        }


        //[HttpPost]
        //public IActionResult Catalog(Guid categoryId)
        //{
        //    //var categories = _categoryService.GetAllAsync().Result;
        //    //ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);

        //    //IEnumerable<Animal> animalsOfCategory;
        //    //if (categoryId != Guid.Empty)
        //    //    animalsOfCategory = _animalService.GetAnimalsByCategory(categoryId).Result;
        //    //else
        //    //    animalsOfCategory = _animalService.GetAllAnimalsWithCategories().Result;

        //    //return View(animalsOfCategory);
        //    if (categoryId == Guid.Empty)
        //        return RedirectToAction(nameof(Catalog));
        //    else
        //        return RedirectToAction(nameof(Catalog), categoryId);
        //}





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}