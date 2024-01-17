using MaximMVC.DAL;
using MaximMVC.Models;
using MaximMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MaximMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Service> services = _context.Services.ToList();
            return View(services);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateVM createVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Service service = new Service() 
            {
                Title=createVM.Title,
                Description = createVM.Description,
                Icon = createVM.Icon
            };


            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");   
        }

        public async Task<IActionResult> Update(int id)
        {
            Service service = _context.Services.Find(id);

            UpdateVM updateVM = new() 
            { 
                Title=service.Title,
                Description=service.Description,
                Icon = service.Icon
            };

            return View(updateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateVM updateVM)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            Service oldService = _context.Services.Find(id);
            oldService.Title= updateVM.Title;
            oldService.Description= updateVM.Description;
            oldService.Icon= updateVM.Icon;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Service service = _context.Services.Find(id);
            _context.Services.Remove(service);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
