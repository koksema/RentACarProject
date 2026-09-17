using CQRS_RentACar.CQRSPattern.Commands.EmployeesCommands;
using CQRS_RentACar.CQRSPattern.Handlers.EmployeeHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.EmployeesHandlers;
using CQRS_RentACar.CQRSPattern.Queries.EmployeesQueries;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly GetEmployeesQueryHandler _getEmployeesQueryHandler;
        private readonly GetEmployeesByIdQueryHandler _getEmployeesByIdQueryHandler;
        private readonly CreateEmployeesCommandHandler _createEmployeesCommandHandler;
        private readonly UpdateEmployeesCommandHandler _updateEmployeesCommandHandler;
        private readonly RemoveEmployeesCommandHandler _removeEmployeesCommandHandler;

        public EmployeesController(
            GetEmployeesQueryHandler getEmployeesQueryHandler,
            GetEmployeesByIdQueryHandler getEmployeesByIdQueryHandler,
            CreateEmployeesCommandHandler createEmployeesCommandHandler,
            UpdateEmployeesCommandHandler updateEmployeesCommandHandler,
            RemoveEmployeesCommandHandler removeEmployeesCommandHandler)
        {
            _getEmployeesQueryHandler = getEmployeesQueryHandler;
            _getEmployeesByIdQueryHandler = getEmployeesByIdQueryHandler;
            _createEmployeesCommandHandler = createEmployeesCommandHandler;
            _updateEmployeesCommandHandler = updateEmployeesCommandHandler;
            _removeEmployeesCommandHandler = removeEmployeesCommandHandler;
        }


        // TÜM PERSONELLER
        public async Task<IActionResult> AllEmployees()
        {
            var value = await _getEmployeesQueryHandler.Handle();

            return View(value);
        }


        // PERSONEL EKLE
        [HttpGet]
        public IActionResult CreateEmployees()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateEmployees(
            CreateEmployeesCommand command)
        {
            await _createEmployeesCommandHandler.Handle(command);

            return RedirectToAction("Index");
        }


        // PERSONEL SİLME ONAY SAYFASI
        [HttpGet]
        public async Task<IActionResult> DeleteEmployees(int id)
        {
            var value =
                await _getEmployeesByIdQueryHandler.Handle(
                    new GetEmployeesByIdQuery(id)
                );

            return View(value);
        }


        // PERSONELİ GERÇEKTEN SİL
        [HttpPost]
        public async Task<IActionResult> DeleteEmployeesConfirmed(
            int employeesId)
        {
            var command =
                new RemoveEmployeesCommand(employeesId);

            await _removeEmployeesCommandHandler.Handle(command);

            return RedirectToAction("Index");
        }


        // PERSONEL GÜNCELLE
        [HttpGet]
        public async Task<IActionResult> UpdateEmployees(int id)
        {
            var value =
                await _getEmployeesByIdQueryHandler.Handle(
                    new GetEmployeesByIdQuery(id)
                );

            return View(value);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateEmployees(
            UpdateEmployeesCommand command)
        {
            await _updateEmployeesCommandHandler.Handle(command);

            return RedirectToAction("Index");
        }
    }
}