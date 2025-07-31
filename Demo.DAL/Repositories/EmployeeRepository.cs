namespace Demo.DAL.Repositories;
public class EmployeeRepository
    : GenericRepository<Employee>
    , IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public IEnumerable<Employee> GetAll(string name)
    {
        throw new NotImplementedException();
    }
}
