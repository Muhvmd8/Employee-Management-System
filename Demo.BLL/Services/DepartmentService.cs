namespace Demo.BLL.Services;
public class DepartmentService(IUnitOfWork unitOfWork) : IDepartmentService
// Injection 
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    //private readonly IDepartmentRepository _unitOfWork.Departments = repository;

    // GetAll 
    public async Task<IEnumerable<DepartmentResponse>> GetAllAsync()
    {
        var departments = await _unitOfWork.Departments.GetAllAsync();

        return departments.Select(department => department.ToResponse());
    }

    // Get 

    public async Task<DepartmentDetailsResponse?> GetByIdAsync(int id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);


        // Manual Mapping 
        // AutoMapper  <<<
        // Mapster 
        // Extension Methods 

        return department is null ? null : department.ToDetailsResponse();
    }


    // Add 

    public async Task<int> AddAsync(DepartmentRequest request)
    {
        var department = request.ToEntity();
        _unitOfWork.Departments.Add(department);
        return await _unitOfWork.SaveChangesAsync();
    }

    // Update 

    public async Task<int> UpdateAsync(DepartmentUpdateRequest request)
    {
        var department = request.ToEntity();
        _unitOfWork.Departments.Update(department);
        return await _unitOfWork.SaveChangesAsync();

    }

    // Delete 
    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department is null)
            return false;
        _unitOfWork.Departments.Delete(department);
        return await _unitOfWork.SaveChangesAsync() > 0 ? true : false;
    }


}
//Ctrl + T 
//Ctrl + ,