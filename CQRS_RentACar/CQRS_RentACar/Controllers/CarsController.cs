using CQRS_RentACar.CQRSPattern.Commands.CarsCommands;
using CQRS_RentACar.CQRSPattern.Handlers.CarsHandlers;
using CQRS_RentACar.CQRSPattern.Queries.CarsQueries;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.Controllers
{
    public class CarsController : Controller
    {
        private readonly GetCarsQueryHandler _getCarsQueryHandler;
        private readonly GetCarsByIdQueryHandler _getCarsByIdQueryHandler;
        private readonly CreateCarsCommandHandler _createCarsCommandHandler;
        private readonly UpdateCarsCommandHandler _updateCarsCommandHandler;
        private readonly RemoveCarsCommandHandler _removeCarsCommandHandler;


        public CarsController(
            GetCarsQueryHandler getCarsQueryHandler,
            GetCarsByIdQueryHandler getCarsByIdQueryHandler,
            CreateCarsCommandHandler createCarsCommandHandler,
            UpdateCarsCommandHandler updateCarsCommandHandler,
            RemoveCarsCommandHandler removeCarsCommandHandler)
        {
            _getCarsQueryHandler = getCarsQueryHandler;
            _getCarsByIdQueryHandler = getCarsByIdQueryHandler;
            _createCarsCommandHandler = createCarsCommandHandler;
            _updateCarsCommandHandler = updateCarsCommandHandler;
            _removeCarsCommandHandler = removeCarsCommandHandler;
        }


        // ==========================================
        // TÜM ARAÇLAR
        // ==========================================

        public async Task<IActionResult> AllCars()
        {
            var value =
                await _getCarsQueryHandler.Handle();

            return View(value);
        }


        // ==========================================
        // ARAÇ EKLE - GET
        // ==========================================

        [HttpGet]
        public IActionResult CreateCars()
        {
            return View();
        }


        // ==========================================
        // ARAÇ EKLE - POST
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> CreateCars(
            CreateCarsCommand command)
        {
            await _createCarsCommandHandler.Handle(command);

            return RedirectToAction("Index");
        }


        // ==========================================
        // ARAÇ SİLME ONAY SAYFASI - GET
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> DeleteCars(int id)
        {
            var value =
                await _getCarsByIdQueryHandler.Handle(
                    new GetCarsByIdQuery(id)
                );

            return View(value);
        }


        // ==========================================
        // ARAÇ SİL - POST
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> DeleteCarsConfirmed(
            int carsId)
        {
            var command =
                new RemoveCarsCommand(carsId);

            await _removeCarsCommandHandler.Handle(command);

            return RedirectToAction("Index");
        }


        // ==========================================
        // ARAÇ GÜNCELLE - GET
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> UpdateCars(int id)
        {
            var value =
                await _getCarsByIdQueryHandler.Handle(
                    new GetCarsByIdQuery(id)
                );

            return View(value);
        }


        // ==========================================
        // ARAÇ GÜNCELLE - POST
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> UpdateCars(
            UpdateCarsCommand command)
        {
            await _updateCarsCommandHandler.Handle(command);

            return RedirectToAction("Index");
        }
    }
}