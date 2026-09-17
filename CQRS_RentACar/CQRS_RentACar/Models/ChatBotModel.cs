using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CQRS_RentACar.Models
{
    public class ChatbotModel
    {
        private readonly string _apiKey;

        public ChatbotModel(string apiKey)
        {
            _apiKey = apiKey;
        }


        public async Task<string> AskGeminiAsync(
            string userPrompt,
            List<CarItemDto> carList)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                return "Gemini API anahtarı bulunamadı.";
            }

            if (string.IsNullOrWhiteSpace(userPrompt))
            {
                return "Lütfen araç arama kriterlerinizi girin.";
            }

            if (carList == null || !carList.Any())
            {
                return "Filoda önerilebilecek araç bulunamadı.";
            }


            using var client = new HttpClient();

            try
            {
                // ==========================================
                // 1 - API KEY'İN ERİŞEBİLDİĞİ MODELİ BUL
                // ==========================================

                var modelName =
                    await GetAvailableModelAsync(client);

                if (string.IsNullOrWhiteSpace(modelName))
                {
                    return
                        "Gemini API için kullanılabilir bir " +
                        "generateContent modeli bulunamadı.";
                }


                // ==========================================
                // 2 - ARAÇ LİSTESİNİ HAZIRLA
                // ==========================================

                var sb = new StringBuilder();

                foreach (var car in carList)
                {
                    sb.AppendLine(
                        $"- ID: {car.Id} | " +
                        $"{car.Brand} {car.Model} | " +
                        $"Kişi: {car.Seat} | " +
                        $"Vites: {car.Transmission} | " +
                        $"Yakıt: {car.Fuel} | " +
                        $"Fiyat: {car.Price:N0} TL"
                    );
                }


                // ==========================================
                // 3 - GEMINI PROMPT
                // ==========================================

                string prompt = $@"
Sen profesyonel bir araç kiralama asistanısın.

Aşağıda şirketimizin mevcut araç filosu bulunmaktadır:

{sb}

Müşterinin isteği:

""{userPrompt}""

Görevin:

1. Müşterinin ihtiyacına en uygun 2 veya 3 aracı seç.
2. Sadece yukarıdaki araç listesinden seçim yap.
3. Listede olmayan herhangi bir araç önerme.
4. Her araç için neden uygun olduğunu kısa ve anlaşılır şekilde açıkla.
5. Yakıt türü, vites, kişi kapasitesi ve fiyat açısından değerlendirme yap.
6. En uygun aracı ayrıca belirt.
7. Türkçe cevap ver.
8. Cevabı düzenli ve okunabilir maddeler halinde hazırla.

Çok uzun cevap verme.
";


                // ==========================================
                // 4 - REQUEST BODY
                // ==========================================

                var requestPayload = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    text = prompt
                                }
                            }
                        }
                    }
                };


                var json =
                    JsonSerializer.Serialize(
                        requestPayload
                    );

                using var jsonContent =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json"
                    );


                // ==========================================
                // 5 - GEMINI REQUEST
                // ==========================================

                var requestUrl =
                    $"https://generativelanguage.googleapis.com/" +
                    $"v1beta/{modelName}:generateContent" +
                    $"?key={_apiKey}";


                using var response =
                    await client.PostAsync(
                        requestUrl,
                        jsonContent
                    );


                var responseString =
                    await response.Content
                        .ReadAsStringAsync();


                // ==========================================
                // 6 - API HATASI
                // ==========================================

                if (!response.IsSuccessStatusCode)
                {
                    return GetGeminiErrorMessage(
                        responseString,
                        (int)response.StatusCode,
                        response.StatusCode.ToString()
                    );
                }


                // ==========================================
                // 7 - GEMINI RESPONSE OKU
                // ==========================================

                using var doc =
                    JsonDocument.Parse(
                        responseString
                    );

                var root =
                    doc.RootElement;


                if (!root.TryGetProperty(
                    "candidates",
                    out var candidates))
                {
                    return
                        "Gemini yanıt verdi ancak " +
                        "öneri oluşturamadı.";
                }


                if (candidates.ValueKind !=
                    JsonValueKind.Array ||
                    candidates.GetArrayLength() == 0)
                {
                    return
                        "Gemini uygun bir öneri oluşturamadı.";
                }


                var firstCandidate =
                    candidates[0];


                if (!firstCandidate.TryGetProperty(
                    "content",
                    out var content))
                {
                    return
                        "Gemini yanıt içeriği alınamadı.";
                }


                if (!content.TryGetProperty(
                    "parts",
                    out var parts))
                {
                    return
                        "Gemini yanıt parçaları alınamadı.";
                }


                if (parts.ValueKind !=
                    JsonValueKind.Array ||
                    parts.GetArrayLength() == 0)
                {
                    return
                        "Gemini boş yanıt döndürdü.";
                }


                var resultBuilder =
                    new StringBuilder();


                foreach (var part in
                         parts.EnumerateArray())
                {
                    if (part.TryGetProperty(
                        "text",
                        out var textElement))
                    {
                        var text =
                            textElement.GetString();

                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            resultBuilder.AppendLine(text);
                        }
                    }
                }


                var result =
                    resultBuilder
                        .ToString()
                        .Trim();


                if (string.IsNullOrWhiteSpace(result))
                {
                    return
                        "Uygun araç önerisi üretilemedi.";
                }


                return result;
            }
            catch (HttpRequestException ex)
            {
                return
                    "Gemini bağlantı hatası: " +
                    ex.Message;
            }
            catch (JsonException ex)
            {
                return
                    "Gemini yanıtı okunamadı: " +
                    ex.Message;
            }
            catch (Exception ex)
            {
                return
                    "Sistem Hatası: " +
                    ex.Message;
            }
        }


        // ==================================================
        // API KEY'İN ERİŞEBİLDİĞİ MODELİ BUL
        // ==================================================

        private async Task<string> GetAvailableModelAsync(
      HttpClient client)
        {
            try
            {
                var url =
                    "https://generativelanguage.googleapis.com/" +
                    $"v1beta/models?key={_apiKey}";

                using var response =
                    await client.GetAsync(url);

                var json =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                using var document =
                    JsonDocument.Parse(json);

                var root =
                    document.RootElement;

                if (!root.TryGetProperty(
                    "models",
                    out var models))
                {
                    return null;
                }

                var availableModels =
                    new List<string>();

                foreach (var model in models.EnumerateArray())
                {
                    if (!model.TryGetProperty(
                        "name",
                        out var nameElement))
                    {
                        continue;
                    }

                    if (!model.TryGetProperty(
                        "supportedGenerationMethods",
                        out var methods))
                    {
                        continue;
                    }

                    bool supportsGenerateContent =
                        methods
                            .EnumerateArray()
                            .Any(x =>
                                string.Equals(
                                    x.GetString(),
                                    "generateContent",
                                    StringComparison.OrdinalIgnoreCase
                                )
                            );

                    if (!supportsGenerateContent)
                    {
                        continue;
                    }

                    var name =
                        nameElement.GetString();

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        availableModels.Add(name);
                    }
                }


                // 1. ÖNCELİK: Gemini 3.6 Flash
                var gemini36Flash =
                    availableModels
                        .FirstOrDefault(x =>
                            x.Contains(
                                "gemini-3.6-flash",
                                StringComparison.OrdinalIgnoreCase
                            )
                        );

                if (!string.IsNullOrWhiteSpace(gemini36Flash))
                {
                    return gemini36Flash;
                }


                // 2. ÖNCELİK: Gemini 3.x Flash
                var gemini3Flash =
                    availableModels
                        .FirstOrDefault(x =>
                            x.Contains(
                                "gemini-3",
                                StringComparison.OrdinalIgnoreCase
                            )
                            &&
                            x.Contains(
                                "flash",
                                StringComparison.OrdinalIgnoreCase
                            )
                        );

                if (!string.IsNullOrWhiteSpace(gemini3Flash))
                {
                    return gemini3Flash;
                }


                // 3. FALLBACK:
                // Eski 2.5 modellerini seçme
                var otherFlash =
                    availableModels
                        .FirstOrDefault(x =>
                            x.Contains(
                                "flash",
                                StringComparison.OrdinalIgnoreCase
                            )
                            &&
                            !x.Contains(
                                "gemini-2.5",
                                StringComparison.OrdinalIgnoreCase
                            )
                        );

                if (!string.IsNullOrWhiteSpace(otherFlash))
                {
                    return otherFlash;
                }


                return availableModels
                    .FirstOrDefault(x =>
                        !x.Contains(
                            "gemini-2.5",
                            StringComparison.OrdinalIgnoreCase
                        )
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "MODEL LİSTESİ HATASI: " +
                    ex.Message
                );

                return null;
            }
        }

        // ==================================================
        // GOOGLE API HATA MESAJINI OKU
        // ==================================================

        private string GetGeminiErrorMessage(
            string responseString,
            int statusCode,
            string statusName)
        {
            try
            {
                using var document =
                    JsonDocument.Parse(
                        responseString
                    );


                var root =
                    document.RootElement;


                if (root.TryGetProperty(
                    "error",
                    out var error))
                {
                    if (error.TryGetProperty(
                        "message",
                        out var message))
                    {
                        return
                            $"Gemini API Hatası " +
                            $"({statusCode} {statusName}): " +
                            message.GetString();
                    }
                }
            }
            catch
            {
                // JSON okunamazsa aşağıdaki genel
                // hata mesajına düşer.
            }


            return
                $"Gemini API Hatası " +
                $"({statusCode} {statusName}): " +
                responseString;
        }
    }


    public class CarItemDto
    {
        public int Id { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        public string CarsImg { get; set; }

        public string Seat { get; set; }

        public string Transmission { get; set; }

        public string Fuel { get; set; }

        public decimal Price { get; set; }
    }
}