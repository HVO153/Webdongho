using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebDongHo.Models;
using WebDongHo.ViewModels;

namespace WebDongHo.Controllers
{
    public class HomeController : Controller
    {
        private readonly DonghodbContext _context;
        public HomeController(DonghodbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>m.Order).Take(2).ToListAsync();
            var slides = await _context.Sliders.Where(m => m.Hide == 0).OrderBy(m =>m.Order).ToListAsync();

            var smaw_prods = await _context.Products.Where(m => m.Hide == 0 && m.IdCat ==2).OrderBy(m => m.Order).Take(3).ToListAsync();
            var smaw_cate_prods = await _context.Catologies.Where(m => m.IdCat ==2).FirstOrDefaultAsync();
            var mecw_prods = await _context.Products.Where(m => m.Hide == 0 && m.IdCat ==1).OrderBy(m => m.Order).Take(3).ToListAsync();
            var mecw_cate_prods = await _context.Catologies.Where(m => m.IdCat ==1).FirstOrDefaultAsync();
            var pinw_prods = await _context.Products.Where(m => m.Hide == 0 && m.IdCat == 3).OrderBy(m => m.Order).Take(3).ToListAsync();
            var pinw_cate_prods = await _context.Catologies.Where(m => m.IdCat == 3).FirstOrDefaultAsync();
            
            var viewModel = new HomeViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Sliders = slides,
                SmaWatchProds = smaw_prods,
                MecWatchProds = mecw_prods,
                PinWatchProds = pinw_prods,
                SmaWCateProds = smaw_cate_prods,
                MecWCateProds = mecw_cate_prods,
                PinWCateProds = pinw_cate_prods,
            };

            return View(viewModel);
        }
        public async Task<IActionResult> _SlidePartial()
        {
            return PartialView();
        }
        public async Task<IActionResult> _ProductPartial()
        {
            return PartialView();
        }
        public async Task<IActionResult> _BlogPartial()
        {
            return PartialView();
        }
        public async Task<IActionResult> _MenuPartial()
        {
            return PartialView();
        }

    }
}
