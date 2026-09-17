using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CQRS_RentACar.Models
{
    public class MessageReplyModel
    {
        private readonly string _apiKey;

        public MessageReplyModel(
            string apiKey)
        {
            _apiKey = apiKey;
        }


        public async Task<string> GenerateReplyAsync(
            string name,
            string surname,
            string subject,
            string messageDetail)
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
            {
                return
                    "Gemini API anahtarı bulunamadı.";
            }


            using var client =
                new HttpClient();


            try
            {
                // Senin projende çalışan Gemini modeli
                var modelName =
                    "models/gemini-3.6-flash";


                var prompt = $@"
Sen profesyonel bir araç kiralama şirketinin
müşteri destek temsilcisisin.

Müşteri bilgileri:

Ad Soyad:
{name} {surname}

Konu:
{subject}

Müşteri Mesajı:
{messageDetail}


Görevin:

1. Müşteriye Türkçe cevap ver.
2. Kurumsal, kibar ve profesyonel ol.
3. Cevabı kısa ve anlaşılır tut.
4. Müşterinin sorusuna doğrudan cevap ver.
5. Bilmediğin veya sistemde doğrulanmamış bir bilgiyi kesinmiş gibi söyleme.
6. Gerekiyorsa müşteri temsilcisinin detaylı inceleme yapacağını belirt.
7. Araç kiralama şirketine uygun bir üslup kullan.
8. Cevabın başında müşteriye adıyla hitap et.
9. Gereksiz uzun açıklamalar yapma.

Sadece müşteriye gönderilecek cevabı üret.
";


                var requestPayload =
                    new
                    {
                        contents =
                            new[]
                            {
                                new
                                {
                                    parts =
                                        new[]
                                        {
                                            new
                                            {
                                                text =
                                                    prompt
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


                var requestUrl =
                    "https://generativelanguage.googleapis.com/" +
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


                if (!response.IsSuccessStatusCode)
                {
                    return GetErrorMessage(
                        responseString,
                        (int)response.StatusCode,
                        response.StatusCode.ToString()
                    );
                }


                using var document =
                    JsonDocument.Parse(
                        responseString
                    );


                var root =
                    document.RootElement;


                if (!root.TryGetProperty(
                    "candidates",
                    out var candidates))
                {
                    return
                        "AI cevabı oluşturulamadı.";
                }


                if (
                    candidates.ValueKind !=
                    JsonValueKind.Array
                    ||
                    candidates.GetArrayLength() == 0
                )
                {
                    return
                        "AI cevabı oluşturulamadı.";
                }


                var candidate =
                    candidates[0];


                if (!candidate.TryGetProperty(
                    "content",
                    out var content))
                {
                    return
                        "AI cevap içeriği alınamadı.";
                }


                if (!content.TryGetProperty(
                    "parts",
                    out var parts))
                {
                    return
                        "AI cevap içeriği alınamadı.";
                }


                if (
                    parts.ValueKind !=
                    JsonValueKind.Array
                    ||
                    parts.GetArrayLength() == 0
                )
                {
                    return
                        "AI boş cevap döndürdü.";
                }


                var text =
                    parts[0]
                        .GetProperty("text")
                        .GetString();


                return
                    string.IsNullOrWhiteSpace(text)
                        ? "AI cevabı oluşturulamadı."
                        : text.Trim();
            }
            catch (Exception ex)
            {
                return
                    "AI cevap sistemi hatası: " +
                    ex.Message;
            }
        }


        private string GetErrorMessage(
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


                if (
                    root.TryGetProperty(
                        "error",
                        out var error)
                    &&
                    error.TryGetProperty(
                        "message",
                        out var message)
                )
                {
                    return
                        $"Gemini API Hatası " +
                        $"({statusCode} {statusName}): " +
                        message.GetString();
                }
            }
            catch
            {
                // Genel hata mesajına düşsün.
            }


            return
                $"Gemini API Hatası " +
                $"({statusCode} {statusName}).";
        }
    }
}