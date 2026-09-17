namespace CQRS_RentACar.CQRSPattern.Commands.EmployeesCommands
{
    public class UpdateEmployeesCommand
    {
        public int EmployeesId { get; set; }
        public string ImgUrl { get; set; }
        public string NameSurname { get; set; }
        public string Profession { get; set; }
    }
}
