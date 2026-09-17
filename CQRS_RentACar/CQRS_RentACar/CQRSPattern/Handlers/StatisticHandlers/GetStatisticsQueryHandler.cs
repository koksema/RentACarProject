using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Results.StatisticResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.StatisticHandlers
{
    public class GetStatisticsQueryHandler
    {
        private readonly DemoContext _context;

        public GetStatisticsQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<GetStatisticsQueryResult> Handle()
        {
            var result = new GetStatisticsQueryResult();

            // 1. Toplam Araç Sayısı
            result.TotalCarCount = await _context.Cars.CountAsync();

            // 2. Toplam Lokasyon Sayısı
            result.TotalLocationCount = await _context.Locations.CountAsync();

            // 3. En Pahalı Araç Modeli (Cars tablosundaki Price alanına göre)
            var expensiveCar = await _context.Cars
                .OrderByDescending(x => x.Price)
                .FirstOrDefaultAsync();

            result.MostExpensiveCar = expensiveCar != null ? $"{expensiveCar.Brand} {expensiveCar.Model}" : "Veri Yok";

            // 4. En Çok Tercih Edilen Lokasyon (String PickUpLocation alanına göre gruplama)
            var popularLocation = await _context.Bookings
                .Where(b => !string.IsNullOrEmpty(b.PickUpLocation))
                .GroupBy(b => b.PickUpLocation)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();

            result.MostPopularLocation = popularLocation ?? "Veri Yok";

            return result;
        }
    }
}
