
using System.Collections.Generic;
using WebDongHo.Models;

namespace WebDongHo.ViewModels
{
    public class BlogViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<Blog> Blogs { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
