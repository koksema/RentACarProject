using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.EmployeesCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.EmployeesHandlers
{
    public class RemoveEmployeesCommandHandler
    {
        private readonly DemoContext _context;

        public RemoveEmployeesCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(RemoveEmployeesCommand command)
        {
            var value = await _context.Employees.FindAsync(command.EmployeesId);
            _context.Employees.Remove(value);
            await _context.SaveChangesAsync();
        }
    }
}
