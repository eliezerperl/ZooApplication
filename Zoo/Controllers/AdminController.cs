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

        // GET: Admin
        public IActionResult Index()
        {
            var animals = _animalService.GetAllAnimalsWithCategories().Result;

            return View(animals);
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            var categories = _categoryService.GetAllAsync().Result;
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Animal animal, IFormFile ImageData)
        {
            animal.Category = await _categoryService.GetByIdAsync(animal.CategoryID);
            //animal.Comments = new List<Comment>();

            //creating file path in order to save file to wwwroot folder
            var root = Path.Combine(_env.WebRootPath, "Uploads");
            Directory.CreateDirectory(root);
            
            var fileName = Path.Combine(root, Guid.NewGuid().ToString("N") + Path.GetExtension(ImageData.FileName));

            //saving in bytes
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                ImageData.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            //manipulating the users inputted image to resize to my liking
            using (Image img = Image.Load(bytes))
            {
                img.Mutate(i => i.Resize(new Size { Width = 200 }));

                img.Save(fileName);
            }

            //giving the animal the fileName of his image
            animal.ImagePath = Path.GetFileName(fileName);

            ModelState.Clear();
            TryValidateModel(animal);
            if (!ModelState.IsValid)
                return View(animal);
            try
            {
                await _animalService.CreateAsync(animal);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/{Guid}
        public IActionResult Edit(Guid id)
        {
            var categories = _categoryService.GetAllAsync().Result;
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            var animal = _animalService.GetByIdAsync(id).Result;
            return View(animal);
        }

        // POST: Admin/Edit/{Guid}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Animal animal)
        {
            try
            {
                await _animalService.UpdateAsync(animal.Id, animal);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/{Guid}
        public IActionResult Delete(Guid id)
        {
            var animalToDelete = _animalService.GetAnimalWithCategory(id).Result;
            return View(animalToDelete);
        }

        // POST: Admin/Delete/{Guid}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAction(Guid id)
        {
            try
            {
                // Deleting the animals image
                var animalToDelete = _animalService.GetByIdAsync(id).Result;
                var imagePath = Path.Combine(_env.WebRootPath, "Uploads", animalToDelete.ImagePath);
                System.IO.File.Delete(imagePath);

                //Deleting the animals comments
                await _commentService.DeleteAllCommentsForAnimal(animalToDelete);

                //Deleting the animal
                await _animalService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
