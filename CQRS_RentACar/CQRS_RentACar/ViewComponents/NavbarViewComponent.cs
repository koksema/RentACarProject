using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}