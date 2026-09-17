using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Results.EmployeesResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.EmployeesHandlers
{
    public class GetEmployeesQueryHandler
    {
        private readonly DemoContext _context;

        public GetEmployeesQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<List<GetEmployeesQueryResult>> Handle()
        {
            var value = await _context.Employees.ToListAsync();
            return value.Select(x => new GetEmployeesQueryResult
            {
                EmployeesId = x.EmployeesId,
                ImgUrl = x.ImgUrl,
                NameSurname = x.NameSurname,
                Profession=x.Profession,

            }).ToList();

        }
    }
}
