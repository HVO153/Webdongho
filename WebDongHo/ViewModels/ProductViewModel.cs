using WebDongHo.Models;

namespace WebDongHo.ViewModels
{
    public class ProductViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Product> Prods { get; set; }
        public String cateName { get; set; }
        public List<Catology> Catologies { get; set; }
        public ProductViewModel()
        {
            Prods = new List<Product>();
            Catologies = new List<Catology>();
        }
    }
}
