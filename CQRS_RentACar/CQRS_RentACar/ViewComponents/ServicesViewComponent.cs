using CQRS_RentACar.CQRSPattern.Handlers.ServicesHandlers;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.ViewComponents
{
    public class ServicesViewComponent:ViewComponent
    {
        private readonly GetServicesQueryHandler _handler;

        public ServicesViewComponent(GetServicesQueryHandler handler)
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
