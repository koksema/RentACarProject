using CQRS_RentACar.CQRSPattern.Commands.TestimonialCommands;
using CQRS_RentACar.CQRSPattern.Handlers.TestimonialHandlers;
using CQRS_RentACar.CQRSPattern.Queries.TestimonialQueries;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly GetTestimonialQueryHandler _getTestimonialQueryHandler;
        private readonly GetTestimonialByIdQueryHandler _getTestimonialByIdQueryHandler;
        private readonly CreateTestimonialCommandHandler _createTestimonialCommandHandler;
        private readonly UpdateTestimonialCommandHandler _updateTestimonialCommandHandler;
        private readonly RemoveTestimonialCommandHandler _removeTestimonialCommandHandler;

        public TestimonialController(GetTestimonialQueryHandler getTestimonialQueryHandler, GetTestimonialByIdQueryHandler getTestimonialByIdQueryHandler, CreateTestimonialCommandHandler createTestimonialCommandHandler, UpdateTestimonialCommandHandler updateTestimonialCommandHandler, RemoveTestimonialCommandHandler removeTestimonialCommandHandler)
        {
            _getTestimonialQueryHandler = getTestimonialQueryHandler;
            _getTestimonialByIdQueryHandler = getTestimonialByIdQueryHandler;
            _createTestimonialCommandHandler = createTestimonialCommandHandler;
            _updateTestimonialCommandHandler = updateTestimonialCommandHandler;
            _removeTestimonialCommandHandler = removeTestimonialCommandHandler;
        }

        public async Task<IActionResult> AllTestimonial()
        {
            var value = await _getTestimonialQueryHandler.Handle();
            return View(value);
        }
        [HttpGet]
        public IActionResult CreateTestimonial()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateTestimonial(CreateTestimonialCommand command)
        {
            await _createTestimonialCommandHandler.Handle(command);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteTestimonial(RemoveTestimonialCommand command)
        {
            await _removeTestimonialCommandHandler.Handle(command);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateTestimonial(int id)
        {
            var value = await _getTestimonialByIdQueryHandler.Handle(new GetTestimonialByIdQuery(id));
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTestimonial(UpdateTestimonialCommand command)
        {
            await _updateTestimonialCommandHandler.Handle(command);
            return RedirectToAction("Index");
        }
    }
}
