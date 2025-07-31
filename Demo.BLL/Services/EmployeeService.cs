namespace Demo.BLL.Services;
public class EmployeeService(IUnitOfWork unitOfWork,
    IMapper mapper,
    IAttachmentService attachmentService)
    : IEmployeeService
{
    //private readonly IEmployeeRepository _unitOfWork.Employees = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly IAttachmentService _attachmentService = attachmentService;

    // GetAll 
    public async Task<IEnumerable<EmployeeResponse>> GetAllAsync(string? SearchValue)
    {
        ///var Employees = _unitOfWork.Employees.GetAll();// FILTRATION => Remote 
        ///Collection<Employee>[Source] => IEnumerable<EmployeeResponse> Dist
        ///return _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(Employees);
        /// FILTRATION => Remote 
        /// Projection => Remote 
        ///var employees = _unitOfWork.Employees.GetAllQueryable().Select(e => new EmployeeResponse
        ///{
        ///    Id = e.Id,
        ///    Age = e.Age,
        ///    Email = e.Email,
        ///    EmployeeType = e.EmployeeType.ToString(),
        ///    Gender = e.Gender.ToString(),
        ///    IsActive = e.IsActive,
        ///    Name = e.Name,
        ///    Salary = e.Salary
        ///});



        if (string.IsNullOrWhiteSpace(SearchValue))
            return await _unitOfWork.Employees.GetAllAsync(e => new EmployeeResponse
            {
                Id = e.Id,
                Age = e.Age,
                Email = e.Email,
                EmployeeType = e.EmployeeType.ToString(),
                Gender = e.Gender.ToString(),
                IsActive = e.IsActive,
                Name = e.Name,
                Salary = e.Salary,
                Department = e.Department.Name,
                Image = e.ImageName
            },
            e => !e.IsDeleted,
            e => e.Department);

        return await _unitOfWork.Employees.GetAllAsync(e => new EmployeeResponse
        {
            Id = e.Id,
            Age = e.Age,
            Email = e.Email,
            EmployeeType = e.EmployeeType.ToString(),
            Gender = e.Gender.ToString(),
            IsActive = e.IsActive,
            Name = e.Name,
            Salary = e.Salary,
            Department = e.Department.Name
        },
          e => !e.IsDeleted && e.Name.ToLower().Contains(SearchValue.ToLower()),
          e => e.Department);


        //return employees;
    }

    // Get 

    public async Task<EmployeeDetailsResponse?> GetByIdAsync(int id)
    {
        var Employee = await _unitOfWork.Employees.GetByIdAsync(id);


        // Manual Mapping 
        // AutoMapper  <<<
        // Mapster 
        // Extension Methods 

        // Employee [Source] => EmployeeDetailsResponse Dist

        return Employee is null ? null : _mapper.Map<Employee, EmployeeDetailsResponse>(Employee);
    }


    // Add 

    public async Task<int> AddAsync(EmployeeRequest request)
    {
        var employee = _mapper.Map<EmployeeRequest, Employee>(request);
        if (request.Image is not null)
            employee.ImageName = await _attachmentService.UploadAsync(request.Image, "Imgs");
        _unitOfWork.Employees.Add(employee);
        return await _unitOfWork.SaveChangesAsync();
    }

    // Update 

    public async Task<int> UpdateAsync(EmployeeUpdateRequest request)
    {
        var Employee = _mapper.Map<EmployeeUpdateRequest, Employee>(request);
        _unitOfWork.Employees.Update(Employee);
        return await _unitOfWork.SaveChangesAsync();

    }

    // Delete 
    public async Task<bool> DeleteAsync(int id)
    {
        var Employee = await _unitOfWork.Employees.GetByIdAsync(id);
        if (Employee is null)
            return false;
        Employee.IsDeleted = true;
        _unitOfWork.Employees.Update(Employee);
        return await _unitOfWork.SaveChangesAsync() > 0 ? true : false;
    }
}
