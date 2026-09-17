namespace CQRS_RentACar.CQRSPattern.Commands.EmployeesCommands
{
    public class CreateEmployeesCommand
    {
        public string ImgUrl { get; set; }
        public string NameSurname { get; set; }
        public string Profession { get; set; }
    }
}
