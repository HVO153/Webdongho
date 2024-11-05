
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebDongHo.Models;
using WebDongHo.ViewModels;

namespace WebDongHo.Controllers
{
    public class BlogController : Controller
    {
        private readonly WebsiteBanAoContext _context;

        public BlogController(WebsiteBanAoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(7).ToListAsync();
            var viewModel = new BlogViewModel
            {
                Menus = menus,
                Blogs = blogs,
            };
            return View(viewModel);
        }
    }
}
