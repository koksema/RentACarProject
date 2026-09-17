using CQRS_RentACar.CQRSPattern.Commands.FeaturesCommands;
using CQRS_RentACar.CQRSPattern.Handlers.FeaturesHandlers;
using CQRS_RentACar.CQRSPattern.Queries.FeaturesQueries;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.Controllers
{
    public class FeaturesController : Controller
    {
        private readonly GetFeaturesQueryHandler _getFeaturesQueryHandler;
        private readonly GetFeaturesByIdQueryHandler _getFeaturesByIdQueryHandler;
        private readonly CreateFeaturesCommandHandler _createFeaturesCommandHandler;
        private readonly UpdateFeaturesCommandHandler _updateFeaturesCommandHandler;
        private readonly RemoveFeaturesCommandHandler _removeFeaturesCommandHandler;

        public FeaturesController(
            GetFeaturesQueryHandler getFeaturesQueryHandler,
            GetFeaturesByIdQueryHandler getFeaturesByIdQueryHandler,
            CreateFeaturesCommandHandler createFeaturesCommandHandler,
            UpdateFeaturesCommandHandler updateFeaturesCommandHandler,
            RemoveFeaturesCommandHandler removeFeaturesCommandHandler)
        {
            _getFeaturesQueryHandler = getFeaturesQueryHandler;
            _getFeaturesByIdQueryHandler = getFeaturesByIdQueryHandler;
            _createFeaturesCommandHandler = createFeaturesCommandHandler;
            _updateFeaturesCommandHandler = updateFeaturesCommandHandler;
            _removeFeaturesCommandHandler = removeFeaturesCommandHandler;
        }


        // TÜM ÖZELLİKLER
        public async Task<IActionResult> AllFeatures()
        {
            var value = await _getFeaturesQueryHandler.Handle();

            return View(value);
        }


        // YENİ ÖZELLİK EKLE
        [HttpGet]
        public IActionResult CreateFeatures()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateFeatures(
            CreateFeaturesCommand command)
        {
            await _createFeaturesCommandHandler.Handle(command);

            return RedirectToAction("Index");
        }


        // SİLME ONAY SAYFASI
        [HttpGet]
        public async Task<IActionResult> DeleteFeatures(int id)
        {
            var value =
                await _getFeaturesByIdQueryHandler.Handle(
                    new GetFeaturesByIdQuery(id)
                );

            return View(value);
        }


        // GERÇEKTEN SİL
        [HttpPost]
        public async Task<IActionResult> DeleteFeaturesConfirmed(
            int featuresId)
        {
            var command =
                new RemoveFeaturesCommand(featuresId);

            await _removeFeaturesCommandHandler.Handle(command);

            return RedirectToAction("Index");
        }


        // GÜNCELLE
        [HttpGet]
        public async Task<IActionResult> UpdateFeatures(int id)
        {
            var value =
                await _getFeaturesByIdQueryHandler.Handle(
                    new GetFeaturesByIdQuery(id)
                );

            return View(value);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateFeatures(
            UpdateFeaturesCommand command)
        {
            await _updateFeaturesCommandHandler.Handle(command);

            return RedirectToAction("Index");
        }
    }
}