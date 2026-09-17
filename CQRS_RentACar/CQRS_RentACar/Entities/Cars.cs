namespace CQRS_RentACar.Entities
{
    public class Cars
    {
        public int CarsId { get; set; }
        public string CarsImg { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Review { get; set; }
        public decimal Price { get; set; }
        public int Seat { get; set; }
        public string Transmission { get; set; }
        public string Fuel { get; set; }
        public string Year { get; set; }
        public int Km { get; set; }

        public ICollection<Booking> Bookings { get; set; }//(Bir arabanın birden fazla rezervasyonu olabilir demektir.)
    }
}
