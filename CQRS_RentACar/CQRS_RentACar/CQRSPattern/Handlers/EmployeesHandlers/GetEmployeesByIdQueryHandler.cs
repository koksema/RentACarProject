using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Queries.EmployeesQueries;
using CQRS_RentACar.CQRSPattern.Results.EmployeesResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.EmployeeHandlers
{
    public class GetEmployeesByIdQueryHandler
    {
        private readonly DemoContext _context;

        public GetEmployeesByIdQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<GetEmployeesByIdQueryResult> Handle(GetEmployeesByIdQuery query)
        {
            var value = await _context.Employees.FirstOrDefaultAsync(x => x.EmployeesId == query.EmployeesId);//Veritabanındaki Employeess tablosuna bak; öyle bir satır (x) bul ki,
            //o satırın EmployeesId değeri benim sana dışarıdan gönderdiğim query.EmployeesId değerine eşit olsun.
            return new GetEmployeesByIdQueryResult
            {
                EmployeesId = value.EmployeesId,
                ImgUrl = value.ImgUrl,
                NameSurname = value.NameSurname,
                Profession = value.Profession,

            };

        }
    }
}
