using WebDongHo.Models;

namespace WebDongHo.ViewModels
{
    public class UserViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<Blog> Blogs { get; set; }
        public User Register { get; set; }
        public List<User> Users { get; set; }  // Danh sách người dùng mới thêm vào


        // Các thuộc tính cần cho đổi mật khẩu
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }


        public UserViewModel()
        {
            Menus = new List<Menu>();
            Blogs = new List<Blog>();
            Register = new User();
            Users = new List<User>();  // Khởi tạo danh sách người dùng
        }
    }
}
