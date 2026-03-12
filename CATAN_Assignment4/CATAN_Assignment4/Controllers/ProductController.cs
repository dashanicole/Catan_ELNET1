using Microsoft.AspNetCore.Mvc;
using CATAN_Assignment4.Models;

namespace CATAN_Assignment4.Controllers
{
    public class ProductController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Product());
        }

        [HttpPost]
        public IActionResult Create(Product model)
        {
            return View(model);
        }
    }
}