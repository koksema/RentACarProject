using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.MessageCommands;
using CQRS_RentACar.CQRSPattern.Handlers.MessageHandlers;
using CQRS_RentACar.CQRSPattern.Queries.MessageQueries;
using CQRS_RentACar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CQRS_RentACar.Controllers
{
    public class MessageController : Controller
    {
        private readonly GetMessageQueryHandler
            _getMessageQueryHandler;

        private readonly GetMessageByIdQueryHandler
            _getMessageByIdQueryHandler;

        private readonly CreateMessageCommandHandler
            _createMessageCommandHandler;

        private readonly UpdateMessageCommandHandler
            _updateMessageCommandHandler;

        private readonly RemoveMessageCommandHandler
            _removeMessageCommandHandler;

        private readonly IConfiguration
            _configuration;

        private readonly DemoContext
            _context;


        public MessageController(
            GetMessageQueryHandler getMessageQueryHandler,
            GetMessageByIdQueryHandler getMessageByIdQueryHandler,
            CreateMessageCommandHandler createMessageCommandHandler,
            UpdateMessageCommandHandler updateMessageCommandHandler,
            RemoveMessageCommandHandler removeMessageCommandHandler,
            IConfiguration configuration,
            DemoContext context)
        {
            _getMessageQueryHandler =
                getMessageQueryHandler;

            _getMessageByIdQueryHandler =
                getMessageByIdQueryHandler;

            _createMessageCommandHandler =
                createMessageCommandHandler;

            _updateMessageCommandHandler =
                updateMessageCommandHandler;

            _removeMessageCommandHandler =
                removeMessageCommandHandler;

            _configuration =
                configuration;

            _context =
                context;
        }


        // =====================================================
        // TÜM MESAJLAR
        // =====================================================

        public async Task<IActionResult> AllMessage()
        {
            var value =
                await _getMessageQueryHandler
                    .Handle();

            return View(value);
        }


        // =====================================================
        // MESAJ OLUŞTUR - GET
        // =====================================================

        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();
        }


        // =====================================================
        // MESAJ OLUŞTUR + GEMINI AI CEVABI
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> CreateMessage(
            CreateMessageCommand command)
        {
            try
            {
                // ---------------------------------------------
                // FORM KONTROLÜ
                // ---------------------------------------------

                if (command == null)
                {
                    return Json(new
                    {
                        success = false,
                        message =
                            "Mesaj bilgileri alınamadı."
                    });
                }


                if (string.IsNullOrWhiteSpace(
                    command.Name))
                {
                    return Json(new
                    {
                        success = false,
                        message =
                            "Lütfen adınızı girin."
                    });
                }


                if (string.IsNullOrWhiteSpace(
                    command.Email))
                {
                    return Json(new
                    {
                        success = false,
                        message =
                            "Lütfen e-posta adresinizi girin."
                    });
                }


                if (string.IsNullOrWhiteSpace(
                    command.Subject))
                {
                    return Json(new
                    {
                        success = false,
                        message =
                            "Lütfen mesaj konusunu girin."
                    });
                }


                if (string.IsNullOrWhiteSpace(
                    command.MessageDeatil))
                {
                    return Json(new
                    {
                        success = false,
                        message =
                            "Lütfen mesajınızı yazın."
                    });
                }


                // ---------------------------------------------
                // 1 - MESAJI VERİTABANINA KAYDET
                // ---------------------------------------------

                var savedMessage =
                    await _createMessageCommandHandler
                        .Handle(command);


                // ---------------------------------------------
                // 2 - GEMINI API KEY
                // ---------------------------------------------

                var apiKey =
                    _configuration[
                        "Gemini:MessageReplyApiKey"
                    ];


                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    return Json(new
                    {
                        success = true,

                        message =
                            "Mesajınız başarıyla alındı.",

                        aiSuccess = false,

                        aiReply =
                            "",

                        warning =
                            "Otomatik cevap sistemi şu anda kullanılamıyor."
                    });
                }


                // ---------------------------------------------
                // 3 - MESSAGE REPLY MODEL
                // ---------------------------------------------

                var aiModel =
                    new MessageReplyModel(
                        apiKey
                    );


                // ---------------------------------------------
                // 4 - GEMINI'DEN CEVAP AL
                // ---------------------------------------------

                var aiReply =
                    await aiModel.GenerateReplyAsync(
                        command.Name,
                        command.Surname,
                        command.Subject,
                        command.MessageDeatil
                    );


                // ---------------------------------------------
                // AI HATA MESAJI MI?
                // ---------------------------------------------

                if (string.IsNullOrWhiteSpace(aiReply))
                {
                    return Json(new
                    {
                        success = true,

                        message =
                            "Mesajınız başarıyla alındı.",

                        aiSuccess = false,

                        aiReply =
                            "",

                        warning =
                            "Otomatik cevap oluşturulamadı."
                    });
                }


                if (
                    aiReply.StartsWith(
                        "Gemini API Hatası",
                        StringComparison.OrdinalIgnoreCase
                    )
                    ||
                    aiReply.StartsWith(
                        "AI cevap sistemi hatası",
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                {
                    Console.WriteLine(
                        "AI MESAJ CEVAP HATASI:"
                    );

                    Console.WriteLine(
                        aiReply
                    );


                    return Json(new
                    {
                        success = true,

                        message =
                            "Mesajınız başarıyla alındı.",

                        aiSuccess = false,

                        aiReply =
                            "",

                        warning =
                            "Otomatik cevap şu anda oluşturulamadı."
                    });
                }


                // ---------------------------------------------
                // 5 - AI CEVABINI AYNI MESAJIN İÇİNE KAYDET
                // ---------------------------------------------

                savedMessage.AiReply =
                    aiReply;


                await _context
                    .SaveChangesAsync();


                // ---------------------------------------------
                // 6 - KULLANICIYA JSON DÖN
                // ---------------------------------------------

                return Json(new
                {
                    success = true,

                    aiSuccess = true,

                    message =
                        "Mesajınız başarıyla gönderildi.",

                    aiReply =
                        aiReply
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "========== MESSAGE HATASI =========="
                );

                Console.WriteLine(
                    ex.ToString()
                );

                Console.WriteLine(
                    "===================================="
                );


                return Json(new
                {
                    success = false,

                    message =
                        "Mesaj gönderilirken bir hata oluştu: "
                        + ex.Message
                });
            }
        }


        // =====================================================
        // MESAJ SİL
        // =====================================================

        public async Task<IActionResult> DeleteMessage(
            RemoveMessageCommand command)
        {
            await _removeMessageCommandHandler
                .Handle(command);

            return RedirectToAction(
                "AllMessage"
            );
        }


        // =====================================================
        // MESAJ GÜNCELLE - GET
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> UpdateMessage(
            int id)
        {
            var value =
                await _getMessageByIdQueryHandler
                    .Handle(
                        new GetMessageByIdQuery(id)
                    );

            return View(value);
        }


        // =====================================================
        // MESAJ GÜNCELLE - POST
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> UpdateMessage(
            UpdateMessageCommand command)
        {
            await _updateMessageCommandHandler
                .Handle(command);

            return RedirectToAction(
                "AllMessage"
            );
        }
    }
}