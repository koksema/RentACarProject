using CQRS_RentACar.CQRSPattern.Commands.SliderCommands;
using CQRS_RentACar.CQRSPattern.Handlers.SliderHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.SlidersHandlers;
using CQRS_RentACar.CQRSPattern.Queries.SliderQueries;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.Controllers
{
    public class SliderController : Controller
    {
        private readonly GetSliderQueryHandler _getSliderQueryHandler;
        private readonly GetSliderByIdQueryHandler _getSliderByIdQueryHandler;
        private readonly CreateSliderCommandHandler _createSliderCommandHandler;
        private readonly UpdateSliderCommandHandler _updateSliderCommandHandler;
        private readonly RemoveSliderCommandHandler _removeSliderCommandHandler;

        public SliderController(GetSliderQueryHandler getSliderQueryHandler, GetSliderByIdQueryHandler getSliderByIdQueryHandler, CreateSliderCommandHandler createSliderCommandHandler, UpdateSliderCommandHandler updateSliderCommandHandler, RemoveSliderCommandHandler removeSliderCommandHandler)
        {
            _getSliderQueryHandler = getSliderQueryHandler;
            _getSliderByIdQueryHandler = getSliderByIdQueryHandler;
            _createSliderCommandHandler = createSliderCommandHandler;
            _updateSliderCommandHandler = updateSliderCommandHandler;
            _removeSliderCommandHandler = removeSliderCommandHandler;
        }

        public async Task<IActionResult> AllSlider()
        {
            var value = await _getSliderQueryHandler.Handle();
            return View(value);
        }
        [HttpGet]
        public IActionResult CreateSlider()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateSlider(CreateSliderCommand command)
        {
            await _createSliderCommandHandler.Handle(command);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteSlider(RemoveSliderCommand command)
        {
            await _removeSliderCommandHandler.Handle(command);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateSlider(int id)
        {
            var value = await _getSliderByIdQueryHandler.Handle(new GetSliderByIdQuery(id));
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSlider(UpdateSliderCommand command)
        {
            await _updateSliderCommandHandler.Handle(command);
            return RedirectToAction("Index");
        }
    }
}
