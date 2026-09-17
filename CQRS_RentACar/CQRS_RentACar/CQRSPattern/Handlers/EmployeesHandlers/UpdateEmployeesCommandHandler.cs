using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.EmployeesCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.EmployeesHandlers
{
    public class UpdateEmployeesCommandHandler
    {
        private readonly DemoContext _context;

        public UpdateEmployeesCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateEmployeesCommand command)
        {
            var value = await _context.Employees.FindAsync(command.EmployeesId);

            value.ImgUrl = command.ImgUrl;
            value.NameSurname = command.NameSurname;
            value.Profession=command.Profession;

            await _context.SaveChangesAsync();
        }
    }
}
