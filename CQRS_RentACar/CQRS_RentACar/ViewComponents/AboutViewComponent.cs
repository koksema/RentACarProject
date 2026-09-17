using CQRS_RentACar.CQRSPattern.Handlers.AboutHandlers;
using CQRS_RentACar.CQRSPattern.Queries.AboutQueries;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.ViewComponents
{
    public class AboutViewComponent:ViewComponent
    {
        private readonly GetAboutByIdQueryHandler _handler;

        public AboutViewComponent(GetAboutByIdQueryHandler handler)
        {
            _handler = handler;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var command = new GetAboutByIdQuery(1);
            var value=await _handler.Handle(command);
            return View(value);
        }
    }
}
