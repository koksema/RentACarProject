using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.EmployeesCommands;
using CQRS_RentACar.Entities;

namespace CQRS_RentACar.CQRSPattern.Handlers.EmployeeHandlers
{
    public class CreateEmployeesCommandHandler
    {
        private readonly DemoContext _context;

        public CreateEmployeesCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(CreateEmployeesCommand command)
        {
            _context.Employees.Add(new Employees
            {
                ImgUrl = command.ImgUrl,
                NameSurname = command.NameSurname,
                Profession = command.Profession,
            });
            await _context.SaveChangesAsync();
        }
    }
}
