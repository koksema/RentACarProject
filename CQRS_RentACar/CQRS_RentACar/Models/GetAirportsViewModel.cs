namespace CQRS_RentACar.Models
{
    public class GetAirportsViewModel
    {

        public class Rootobject
        {
            public string status { get; set; }
            public string message { get; set; }
            public string messageTR { get; set; }
            public int systemTime { get; set; }
            public string endpoint { get; set; }
            public int rowCount { get; set; }
            public int creditUsed { get; set; }
            public Pagination pagination { get; set; }
            public Datum[] data { get; set; }
        }

        public class Pagination
        {
            public int total { get; set; }
            public int per_page { get; set; }
            public int current_page { get; set; }
            public int last_page { get; set; }
            public int from { get; set; }
            public int to { get; set; }
        }

        public class Datum
        {
            public string name { get; set; }
            public string iata { get; set; }
            public string iaco { get; set; }
            public float latitude { get; set; }
            public float longitude { get; set; }
            public string country { get; set; }
            public string region { get; set; }
            public int alt { get; set; }
            public int elevation { get; set; }
            public string timezone { get; set; }
            public string website { get; set; }
            public string wikipedia { get; set; }
        }

    }
}
