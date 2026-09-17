using CQRS_RentACar.CQRSPattern.Commands.ServicesCommands;
using CQRS_RentACar.CQRSPattern.Handlers.ServicesHandlers;
using CQRS_RentACar.CQRSPattern.Queries.ServicesQueries;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.Controllers
{
    public class ServicesController : Controller
    {
        private readonly GetServicesQueryHandler _getServicesQueryHandler;
        private readonly GetServicesByIdQueryHandler _getServicesByIdQueryHandler;
        private readonly CreateServicesCommandHandler _createServicesCommandHandler;
        private readonly UpdateServicesCommandHandler _updateServicesCommandHandler;
        private readonly RemoveServicesCommandHandler _removeServicesCommandHandler;

        public ServicesController(GetServicesQueryHandler getServicesQueryHandler, GetServicesByIdQueryHandler getServicesByIdQueryHandler, CreateServicesCommandHandler createServicesCommandHandler, UpdateServicesCommandHandler updateServicesCommandHandler, RemoveServicesCommandHandler removeServicesCommandHandler)
        {
            _getServicesQueryHandler = getServicesQueryHandler;
            _getServicesByIdQueryHandler = getServicesByIdQueryHandler;
            _createServicesCommandHandler = createServicesCommandHandler;
            _updateServicesCommandHandler = updateServicesCommandHandler;
            _removeServicesCommandHandler = removeServicesCommandHandler;
        }

        public async Task<IActionResult> AllServices()
        {
            var value = await _getServicesQueryHandler.Handle();
            return View(value);
        }
        [HttpGet]
        public IActionResult CreateServices()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateServices(CreateServicesCommand command)
        {
            await _createServicesCommandHandler.Handle(command);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteServices(RemoveServicesCommand command)
        {
            await _removeServicesCommandHandler.Handle(command);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateServices(int id)
        {
            var value = await _getServicesByIdQueryHandler.Handle(new GetServicesByIdQuery(id));
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateServices(UpdateServicesCommand command)
        {
            await _updateServicesCommandHandler.Handle(command);
            return RedirectToAction("Index");
        }
    }
}
