using CQRS_RentACar.CQRSPattern.Handlers.TestimonialHandlers;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.ViewComponents
{
    public class TestimonialViewComponent:ViewComponent
    {
        private readonly GetTestimonialQueryHandler _handler;

        public TestimonialViewComponent(GetTestimonialQueryHandler handler)
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
