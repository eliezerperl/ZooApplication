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


        public async Task<IActionResult> Index()
        {
            ////FOR LIRON TO ADD CATEGORIES
            //var cat1 = new Category { Name="Birds" };
            //var cat2 = new Category { Name="Mammals" };
            //var cat3 = new Category { Name="Reptiles" };
            //await _categoryService.CreateAsync(cat1);
            //await _categoryService.CreateAsync(cat2);
            //await _categoryService.CreateAsync(cat3);

            var animals = await _animalService.GetTopTwoAnimalsWithCategories();

            return View(animals);
        }


        public async Task<IActionResult> Details(Guid Id)
        {
            var animal = await _animalService.GetAnimalWithCategory(Id);
            animal.Comments = await _commentService.GetCommentsForAnimal(animal);
            return View(animal);
        }


        [HttpPost]
        public async Task<IActionResult> PostComment(string comment, Guid Id)
        {
            var userComment = new Comment
            {
                Content = comment,
                AnimalID = Id,
            };
            await _commentService.CreateAsync(userComment);
            return RedirectToAction(nameof(Details), new {Id});
        }


        public async Task<IActionResult> Catalog(Guid? categoryId)
        {
            IEnumerable<Animal> animals;

            if (categoryId.HasValue && categoryId != Guid.Empty) 
                animals = await _animalService.GetAnimalsByCategory(categoryId.Value);
            else
                animals = await _animalService.GetAllAnimalsWithCategories();

            var categories = await _categoryService.GetAllAsync();
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



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}