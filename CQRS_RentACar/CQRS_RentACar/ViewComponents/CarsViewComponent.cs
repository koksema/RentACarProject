using CQRS_RentACar.CQRSPattern.Handlers.CarsHandlers;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.ViewComponents
{
    public class CarsViewComponent:ViewComponent
    {
        private readonly GetCarsQueryHandler _handler;

        public CarsViewComponent(GetCarsQueryHandler handler)
        {
            _handler = handler;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var value = await _handler.Handle();
            return View(value);
        }
    }
}
