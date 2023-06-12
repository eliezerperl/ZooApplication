using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZooDAL.Entities;
using ZooDAL.Services.Intefaces;

namespace Zoo.Controllers
{
    public class AdminController : Controller
    {
        IAnimalService _animalService;
        ICommentService _commentService;
        ICategoryService _categoryService;
        IWebHostEnvironment _env;

        public AdminController(IWebHostEnvironment environment, IAnimalService animalService, ICommentService commentService, ICategoryService categoryService)
        {
            _animalService = animalService;
            _commentService = commentService;
            _categoryService = categoryService;
            _env = environment;
        }

        // GET: AdminController
        public IActionResult Index()
        {
            var animals = _animalService.GetAllAnimalsWithCategories().Result;

            return View(animals);
        }

        //// GET: AdminController/Details/5
        //public IActionResult Details(Guid id)
        //{
        //    var animal = _animalService.GetByIdAsync(id).Result;
        //    animal.Comments = _commentService.GetCommentsForAnimal(animal).Result;
        //    return View(animal);
        //}

        // GET: AdminController/Create
        public IActionResult Create()
        {
            var categories = _categoryService.GetAllAsync().Result;
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Animal animal, IFormFile ImageData)
        {
            animal.Category = _categoryService.GetByIdAsync(animal.CategoryID).Result;


            //saving file to wwwroot folder
            var root = Path.Combine(_env.WebRootPath, "Uploads");
            Directory.CreateDirectory(root);
            
            var fileName = Path.Combine(root, Guid.NewGuid().ToString("N") + Path.GetExtension(ImageData.FileName));

            //saving to bytes
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                ImageData.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            //munipulating the users inputted image to resize to my liking
            using (Image img = Image.Load(bytes))
            {
                img.Mutate(i => i.Resize(new Size { Width = 200 }));

                img.Save(fileName);
            }

            //using (var fileStream = new FileStream(fileName, FileMode.Create))
            //{
            //    ImageData.CopyTo(fileStream);
            //}

            //giving the animal the relative path to his image
            animal.ImagePath = Path.GetFileName(fileName);

            ModelState.Clear();
            TryValidateModel(animal);
            if (!ModelState.IsValid)
                return View(animal);
            try
            {
                _animalService.CreateAsync(animal);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public IActionResult Edit(Guid id)
        {
            var categories = _categoryService.GetAllAsync().Result;
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            var animal = _animalService.GetByIdAsync(id).Result;
            return View(animal);
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Animal animal)
        {
            try
            {
                await _animalService.UpdateAsync(id, animal);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        public IActionResult Delete(Guid id)
        {
            var animalToDelete = _animalService.GetAnimalWithCategory(id).Result;
            return View(animalToDelete);
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id, IFormCollection collection)
        {
            try
            {
                var animalToDelete = _animalService.GetByIdAsync(id).Result;
                var imagePath = Path.Combine(_env.WebRootPath, "Uploads", animalToDelete.ImagePath);
                System.IO.File.Delete(imagePath);
                _animalService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
