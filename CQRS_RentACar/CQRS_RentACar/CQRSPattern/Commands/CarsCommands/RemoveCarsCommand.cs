namespace CQRS_RentACar.CQRSPattern.Commands.CarsCommands
{
    public class RemoveCarsCommand
    {
        public int CarsId { get; set; }

        public RemoveCarsCommand(int carsId)
        {
            CarsId = carsId;
        }
    }
}
