using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDongHo.Models;
using WebDongHo.ViewModels;

namespace WebDongHo.Controllers
{
    public class ProductController: Controller
    {



        private readonly DonghodbContext _context;
        public ProductController(DonghodbContext context)
        {
            _context = context;
        }



        public async Task<IActionResult> Index()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();
            var prods = await _context.Products.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Prods = prods,
            };
            return View(viewModel);
        }





        public async Task<IActionResult> _MenuPartial()
        {
            return PartialView();
        }
        public async Task<IActionResult> _BlogPartial()
        {
            return PartialView();
        }





        public async Task<IActionResult> CateProd(string slug, long id)
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).Take(2).ToListAsync();
            var cateProds = await _context.Catologies
            .Where(cp => cp.IdCat == id && cp.Link == slug).FirstOrDefaultAsync();
            if (cateProds == null)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = "CateProd Error",
                };
                return View("Error", errorViewModel);
            }
            var prods = await _context.Products
                .Where(m => m.Hide == 0 && m.IdCat == cateProds.IdCat)
                .OrderBy(m => m.Order).ToListAsync();
            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Prods = prods,
                cateName = cateProds.NameCat,
            };
            return View(viewModel);
        }





        public async Task<IActionResult> ProdDetail(string slug, long id)
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).Take(2).ToListAsync();
            var prods = await _context.Products.Where(m => m.Link == slug && m.IdPro ==
            id).ToListAsync();
            if (prods == null)
            {
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = "Product Error",
                };
                return View("Error", errorViewModel);
            }
            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Prods = prods,
            };
            return View(viewModel);
        }






        // Trang quản lý sản phẩm
        [Authorize(Roles = "1")]
        public async Task<IActionResult> ManageProduct()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();
            var prods = await _context.Products.OrderBy(m => m.Order).ToListAsync();

            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Prods = prods
            };

            return View(viewModel);
        }





        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create()
        {
            
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();
            var prods = await _context.Products.OrderBy(m => m.Order).ToListAsync();



            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Prods = prods,
                Catologies = _context.Catologies.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product,IFormFile Img1, IFormFile Img2, IFormFile Img3)
        {
            if (ModelState.IsValid)
            {
                // Xử lý hình ảnh 1
                if (Img1 != null && Img1.Length > 0)
                {
                    var fileName1 = Path.GetFileName(Img1.FileName);
                    var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName1);
                    using (var stream = new FileStream(filePath1, FileMode.Create))
                    {
                        await Img1.CopyToAsync(stream);
                    }
                    product.Img1 = fileName1;
                }

                // Xử lý hình ảnh 2
                if (Img2 != null && Img2.Length > 0)
                {
                    var fileName2 = Path.GetFileName(Img2.FileName);
                    var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName2);
                    using (var stream = new FileStream(filePath2, FileMode.Create))
                    {
                        await Img2.CopyToAsync(stream);
                    }
                    product.Img2 = fileName2;
                }

                // Xử lý hình ảnh 3
                if (Img3 != null && Img3.Length > 0)
                {
                    var fileName3 = Path.GetFileName(Img3.FileName);
                    var filePath3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName3);
                    using (var stream = new FileStream(filePath3, FileMode.Create))
                    {
                        await Img3.CopyToAsync(stream);
                    }
                    product.Img3 = fileName3;
                }

                // Lưu sản phẩm vào cơ sở dữ liệu
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageProduct));
            }
           
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();
            var prods = await _context.Products.OrderBy(m => m.Order).ToListAsync();

            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Prods = prods
            };

            return View(viewModel);
        }




        [Authorize(Roles = "1")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

           
            var catologies = await _context.Catologies.Where(c => c.Hide == 0).OrderBy(c => c.NameCat).ToListAsync();

            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();

            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Prods = new List<Product> { product },
                Catologies = catologies // Thêm danh sách Catologies
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile Img1, IFormFile Img2, IFormFile Img3)
        {
            if (id != product.IdPro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra và xử lý ảnh 1
                    if (Img1 != null && Img1.Length > 0)
                    {
                        var fileName1 = Path.GetFileName(Img1.FileName);
                        var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName1);
                        using (var stream = new FileStream(filePath1, FileMode.Create))
                        {
                            await Img1.CopyToAsync(stream);
                        }
                        product.Img1 = fileName1; // Cập nhật Img1
                    }

                    // Kiểm tra và xử lý ảnh 2
                    if (Img2 != null && Img2.Length > 0)
                    {
                        var fileName2 = Path.GetFileName(Img2.FileName);
                        var filePath2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName2);
                        using (var stream = new FileStream(filePath2, FileMode.Create))
                        {
                            await Img2.CopyToAsync(stream);
                        }
                        product.Img2 = fileName2; // Cập nhật Img2
                    }

                    // Kiểm tra và xử lý ảnh 3
                    if (Img3 != null && Img3.Length > 0)
                    {
                        var fileName3 = Path.GetFileName(Img3.FileName);
                        var filePath3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName3);
                        using (var stream = new FileStream(filePath3, FileMode.Create))
                        {
                            await Img3.CopyToAsync(stream);
                        }
                        product.Img3 = fileName3; // Cập nhật Img3
                    }

                    // Cập nhật sản phẩm vào cơ sở dữ liệu
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.IdPro == product.IdPro))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(ManageProduct));
            }

            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();
            var catologies = await _context.Catologies.Where(c => c.Hide == 0).OrderBy(c => c.NameCat).ToListAsync();

            var viewModel = new ProductViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Catologies = catologies
            };

            return View(viewModel);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = ".......................Xóa sản phẩm thành công rồi!     └⁠(⁠ ⁠＾⁠ω⁠＾⁠)⁠」.......〜⁠(⁠꒪⁠꒳⁠꒪⁠)⁠〜........(⁠~⁠‾⁠▿⁠‾⁠)⁠~";
            return RedirectToAction(nameof(ManageProduct));
        }



















    }
}
