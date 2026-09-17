using CQRS_RentACar.CQRSPattern.Handlers.EmployeesHandlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CQRS_RentACar.ViewComponents
{
    public class EmployeesViewComponent:ViewComponent
    {
        private readonly GetEmployeesQueryHandler _handler;

        public EmployeesViewComponent(GetEmployeesQueryHandler handler)
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
