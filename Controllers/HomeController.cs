using MaximMVC.DAL;
using MaximMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MaximMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Service> services = _context.Services.ToList(); 
            return View(services);
        }

    }
}