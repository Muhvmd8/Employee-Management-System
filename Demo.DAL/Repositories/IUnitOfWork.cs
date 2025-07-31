namespace Demo.DAL.Repositories;
public interface IUnitOfWork
{
    IEmployeeRepository Employees { get; }
    //IGenericRepository<Employee> Employees { get; }
    IDepartmentRepository Departments { get; }
    Task<int> SaveChangesAsync();

}
