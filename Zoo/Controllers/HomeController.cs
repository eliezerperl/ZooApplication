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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostComment(string comment, Guid Id)
        {
            if (string.IsNullOrEmpty(comment))
            {
                ModelState.AddModelError("comment", "You must write a comment");
                return RedirectToAction(nameof(Details), new { Id });
            }

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