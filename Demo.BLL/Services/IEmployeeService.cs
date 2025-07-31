namespace Demo.BLL.Services;
public interface IEmployeeService
{
    Task<int> AddAsync(EmployeeRequest request);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<EmployeeResponse>> GetAllAsync(string? SearchValue);
    Task<EmployeeDetailsResponse?> GetByIdAsync(int id);
    Task<int> UpdateAsync(EmployeeUpdateRequest request);
}
