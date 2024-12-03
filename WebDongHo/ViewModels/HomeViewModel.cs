using WebDongHo.Models;

namespace WebDongHo.ViewModels
{
    public class HomeViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Slider> Sliders { get; set; }

        public List<Product> MecWatchProds { get; set; }
        public List<Product> SmaWatchProds { get; set; }
        public List<Product> PinWatchProds { get; set; }
        public List<Product> LighteWatchProds { get; set; }
 
        public Catology MecWCateProds { get; set; }
        public Catology SmaWCateProds { get; set; }
        public Catology PinWCateProds { get; set; }
        public Catology LighteWCateProds { get;set; }   

    }
}
