namespace CQRS_RentACar.CQRSPattern.Commands.LocationCommands
{
    public class RemoveLocationCommand
    {
        public int LocationID { get; set; }
        public string Name { get; set; }
        public string Iata { get; set; }
        public string Iaco { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Country { get; set; }
        public int Elevation { get; set; }
        public string Timezone { get; set; }
    }
}
