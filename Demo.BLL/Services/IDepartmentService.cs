namespace Demo.BLL.Services;
public interface IDepartmentService
{
    Task<int> AddAsync(DepartmentRequest request);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<DepartmentResponse>> GetAllAsync();
    Task<DepartmentDetailsResponse?> GetByIdAsync(int id);
    Task<int> UpdateAsync(DepartmentUpdateRequest request);
}