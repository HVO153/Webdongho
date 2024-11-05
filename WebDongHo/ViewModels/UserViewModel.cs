using WebDongHo.Models;

namespace WebDongHo.ViewModels
{
    public class UserViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<Blog> Blogs { get; set; }
        public User Register { get; set; }
        public UserViewModel()
        {
            Menus = new List<Menu>(); 
            Blogs = new List<Blog>();
            Register = new User();
        }
    }
}
