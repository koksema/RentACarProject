namespace CQRS_RentACar.CQRSPattern.Queries.EmployeesQueries
{
    public class GetEmployeesByIdQuery
    {
        public int EmployeesId { get; set; }

        public GetEmployeesByIdQuery(int employeesId)
        {
            EmployeesId = employeesId;
        }
    }
}
