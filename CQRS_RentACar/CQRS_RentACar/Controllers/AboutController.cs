using System.Threading.Tasks;
using CQRS_RentACar.CQRSPattern.Commands.AboutCommands;
using CQRS_RentACar.CQRSPattern.Handlers.AboutHandlers;
using CQRS_RentACar.CQRSPattern.Queries.AboutQueries;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.Controllers
{
    public class AboutController : Controller
    {
        private readonly GetAboutQueryHandler _getAboutQueryHandler;
        private readonly CreateAboutCommandHandler _createAboutCommandHandler;
        private readonly GetAboutByIdQueryHandler _getAboutByIdQueryHandler;
        private readonly RemoveAboutCommandHandler _removeAboutCommandHandler;
        private readonly UpdateAboutCommandHandler _updateAboutCommandHandler;

        public AboutController(
            GetAboutQueryHandler getAboutQueryHandler,
            CreateAboutCommandHandler createAboutCommandHandler,
            GetAboutByIdQueryHandler getAboutByIdQueryHandler,
            RemoveAboutCommandHandler removeAboutCommandHandler,
            UpdateAboutCommandHandler updateAboutCommandHandler)
        {
            _getAboutQueryHandler = getAboutQueryHandler;
            _createAboutCommandHandler = createAboutCommandHandler;
            _getAboutByIdQueryHandler = getAboutByIdQueryHandler;
            _removeAboutCommandHandler = removeAboutCommandHandler;
            _updateAboutCommandHandler = updateAboutCommandHandler;
        }

        public async Task<IActionResult> AllAbouts()
        {
            var value = await _getAboutQueryHandler.Handle();
            return View(value);
        }

        [HttpGet]
        public IActionResult CreateAbout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAbout(CreateAboutCommand command)
        {
            await _createAboutCommandHandler.Handle(command);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAbout(int id)
        {
            var value = await _getAboutByIdQueryHandler
                .Handle(new GetAboutByIdQuery(id));

            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAboutConfirmed(int aboutId)
        {
            await _removeAboutCommandHandler
                .Handle(new RemoveAboutCommand(aboutId));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateAbout(int id)
        {
            var value = await _getAboutByIdQueryHandler
                .Handle(new GetAboutByIdQuery(id));

            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAbout(UpdateAboutCommand command)
        {
            await _updateAboutCommandHandler.Handle(command);

            return RedirectToAction("Index");
        }
    }
}