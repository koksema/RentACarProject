namespace CQRS_RentACar.Models
{
    public class CalculationModel
    {
        public string CarId { get; set; }
        public string PickUpLocation { get; set; }  // Örn: RapidAPI'den dönen DestId veya CityId
        public string DropOffLocation { get; set; }

        public DateTime PickUpDate { get; set; }
        public string PickUpTime { get; set; }

        public DateTime DropOffDate { get; set; }
        public string DropOffTime { get; set; }

        // API Sonucu Hesaplanan Değerler
        public decimal TotalPrice { get; set; }
        public int TotalDays { get; set; }
        public string Currency { get; set; }
    }
}
