namespace Demo.DAL.Repositories;
public interface IEmployeeRepository : IGenericRepository<Employee>
{
    public IEnumerable<Employee> GetAll(string name);
}
