namespace CQRS_RentACar.CQRSPattern.Results.EmployeesResults
{
    public class GetEmployeesQueryResult
    {
        public int EmployeesId { get; set; }
        public string ImgUrl { get; set; }
        public string NameSurname { get; set; }
        public string Profession { get; set; }
    }
}
