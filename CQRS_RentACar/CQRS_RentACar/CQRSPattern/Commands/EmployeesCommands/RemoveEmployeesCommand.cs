namespace CQRS_RentACar.CQRSPattern.Commands.EmployeesCommands
{
    public class RemoveEmployeesCommand
    {
        public int EmployeesId { get; set; }

        public RemoveEmployeesCommand(int employeesId)
        {
            EmployeesId = employeesId;
        }
    }
}
