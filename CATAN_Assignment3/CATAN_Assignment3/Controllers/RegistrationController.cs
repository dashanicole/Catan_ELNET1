using Microsoft.AspNetCore.Mvc;
using CATAN_Assignment3.Models;

namespace CATAN_Assignment3.Controllers
{
    public class RegistrationController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View(new StudentRegistration());
        }

        [HttpPost]
        public IActionResult Register(StudentRegistration model)
        {
            return View(model);
        }
    }
}