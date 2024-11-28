using Microsoft.AspNetCore.Mvc;
using WebDongHo.ViewModels;
using WebDongHo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace WebDongHo.Controllers
{
        public class UserController : Controller
        {
            private readonly DonghodbContext _context;
            public UserController(DonghodbContext context)
            {
                _context = context;
            }
            [HttpGet]
            public async Task<IActionResult> Register()
            {
                if (User.Identity.IsAuthenticated)
                {
                    // Nếu đã đăng nhập, chuyển hướng người dùng đến trang chủ
                    return RedirectToAction("Index", "Home");
                }
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>m.Order).ToListAsync();
                var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>m.Order).Take(2).ToListAsync();
                var viewModel = new UserViewModel
                {
                    Menus = menus,
                    Blogs = blogs,
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
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Nếu đã đăng nhập, chuyển hướng người dùng đến trang chủ
                return RedirectToAction("Index", "Home");
            }
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).Take(2).ToListAsync();
            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserViewModel model)
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).Take(2).ToListAsync();
            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Register = model.Register,
            };
            if (model.Register != null)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username ==
                model.Register.Username);
                if (existingUser != null)
                {
                    ViewBag.ErrorMessage = "Tên đăng nhập đã tồn tại.";
                    return View(viewModel);
                }
                model.Register.Password =
                BCrypt.Net.BCrypt.HashPassword(model.Register.Password);
                model.Register.Permission = 0;
                model.Register.Hide = 0;
                _context.Users.Add(model.Register);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "User");
            }
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).Take(2).ToListAsync();
            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Register = model.Register,
            };
            if (model.Register != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username ==
                model.Register.Username);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Register.Password,
                user.Password))
                {
                    var claims = new List<Claim>
                    {   new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, user.Permission.ToString()),
                    };
                    var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                    };
                    await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
                    return View(viewModel);
                }
            }
            return View(viewModel);
        }
        public async Task<IActionResult> Info()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).Take(2).ToListAsync();
            var users = new User();
            if (User.Identity.IsAuthenticated)
            {
                string username = User.Identity.Name;
                if (username != null)
                {
                    users = await _context.Users.FirstOrDefaultAsync(m => m.Username ==
                    username);
                }
            }
            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Register = users,
            };
            return View(viewModel);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }



        [HttpGet]
        public async Task<IActionResult> EditInfo()
        {
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m =>
            m.Order).Take(2).ToListAsync();
            var users = new User();
            if (User.Identity.IsAuthenticated)
            {
                string username = User.Identity.Name;
                if (username != null)
                {
                    users = await _context.Users.FirstOrDefaultAsync(m => m.Username ==
                    username);
                }
            }
            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Register = users,
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInfo(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy thông tin người dùng từ cơ sở dữ liệu
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Register.Username);

                    if (user != null)
                    {
                        // Cập nhật thông tin từ form chỉnh sửa
                        user.Username = model.Register.Username;
                        user.Name = model.Register.Name;
                        user.Address = model.Register.Address;
                        user.Email = model.Register.Email;
                        user.Phone = model.Register.Phone;

                        // Kiểm tra xem có mật khẩu mới được cung cấp hay không
                        if (!string.IsNullOrEmpty(model.Register.Password))
                        {
                            // Mã hóa mật khẩu mới trước khi lưu vào cơ sở dữ liệu
                            user.Password = BCrypt.Net.BCrypt.HashPassword(model.Register.Password);

                            // Lưu thay đổi vào cơ sở dữ liệu
                            _context.Users.Update(user);
                            await _context.SaveChangesAsync();
                        }

                        // Đăng xuất người dùng sau khi cập nhật thành công
                        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                        // Chuyển hướng đến trang đăng nhập
                        return RedirectToAction("Login", "User");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Người dùng không tồn tại.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Đã xảy ra lỗi khi cập nhật thông tin người dùng: " + ex.Message;
                }
            }

            return View("EditInfo", model);
        }


        [Authorize(Roles = "1")] // Chỉ admin mới được truy cập trang này
        public async Task<IActionResult> AdminDashboard()
        {
            // Lấy danh sách menu từ cơ sở dữ liệu
            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();

            // Khởi tạo ViewModel và truyền menu vào View
            var viewModel = new UserViewModel
            {
                Menus = menus, // Truyền dữ liệu menu vào ViewModel
                Blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync()
            };

            return View(viewModel); // Trả về view với ViewModel chứa menu
        }






        [Authorize(Roles = "1")] // Chỉ admin mới được truy cập
        public async Task<IActionResult> ManageAccounts()
        {
            // Lấy danh sách người dùng từ cơ sở dữ liệu
            var users = await _context.Users.ToListAsync();

            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();

            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Users = users
            };

            return View(viewModel); // Trả về view với danh sách người dùng
        }





        [HttpGet]
        [Authorize(Roles = "1")] // Chỉ admin mới được truy cập
        public async Task<IActionResult> ChangePassword(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var menus = await _context.Menus.Where(m => m.Hide == 0).OrderBy(m => m.Order).ToListAsync();
            var blogs = await _context.Blogs.Where(m => m.Hide == 0).OrderBy(m => m.Order).Take(2).ToListAsync();

            var viewModel = new UserViewModel
            {
                Menus = menus,
                Blogs = blogs,
                Register = user // Truyền thông tin người dùng vào view
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UserViewModel model)
        {
            if (model.Register != null && !string.IsNullOrEmpty(model.Register.Password))
            {
                var user = await _context.Users.FindAsync(model.Register.IdUsers);
                if (user != null)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(model.Register.Password);
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                    return RedirectToAction("ManageAccounts");
                }
            }

            TempData["ErrorMessage"] = "Đã xảy ra lỗi. Vui lòng thử lại.";
            return View(model);
        }

















    }
}
