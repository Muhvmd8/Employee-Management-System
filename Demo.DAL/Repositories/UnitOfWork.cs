namespace Demo.DAL.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context; // unManaged Res
    private readonly Lazy<IEmployeeRepository> _employeeRepository;
    private readonly Lazy<IDepartmentRepository> _departmentRepository;

    //private readonly Func<IEmployeeRepository> _employeeRepositoryFactory;
    //private readonly Func<IDepartmentRepository> _departmentRepositoryFactory;
    public UnitOfWork(ApplicationDbContext context,
        Func<IEmployeeRepository> employeeRepositoryFactory,
        Func<IDepartmentRepository> departmentRepositoryFactory) //  DI 
    {
        _context = context;
        //_employeeRepositoryFactory = employeeRepositoryFactory;
        //_departmentRepositoryFactory = departmentRepositoryFactory;

        _employeeRepository = new Lazy<IEmployeeRepository>(() =>
        {
            Console.WriteLine("Employee Repository is Being Initialized");
            return new EmployeeRepository(context);
        });
        _departmentRepository = new Lazy<IDepartmentRepository>(() =>
        {
            Console.WriteLine("Department Repository is Being Initialized");
            return new DepartmentRepository(context);
        });
    }

    public IEmployeeRepository Employees => _employeeRepository.Value;
    public IDepartmentRepository Departments => _departmentRepository.Value;

    //private IEmployeeRepository _employeeRepository;
    //private IDepartmentRepository _departmentRepository;
    //public IEmployeeRepository Employees => _employeeRepository ??= _employeeRepositoryFactory.Invoke();
    //public IDepartmentRepository Departments => _departmentRepository ??= _departmentRepositoryFactory.Invoke();

    //public void Dispose() => _context.Dispose();
    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}
