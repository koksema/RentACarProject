using CQRS_RentACar.CQRSPattern.Handlers.EmployeesHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.FeaturesHandlers;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.ViewComponents
{
    public class FeaturesViewComponent:ViewComponent
    {
        private readonly GetFeaturesQueryHandler _handler;

        public FeaturesViewComponent(GetFeaturesQueryHandler handler)
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
