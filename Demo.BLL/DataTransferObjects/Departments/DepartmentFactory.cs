namespace Demo.BLL.DataTransferObjects.Departments;
public static class DepartmentFactory
{
    public static DepartmentResponse ToResponse(this Department department) => new()
    {
        Id = department.Id,
        Name = department.Name,
        Description = department.Description,
        CreatedOn = DateOnly.FromDateTime(department.CreatedOn),
        Code = department.Code
    };
    public static DepartmentDetailsResponse ToDetailsResponse(this Department department) => new()
    {
        Id = department.Id,
        Name = department.Name,
        Description = department.Description,
        CreatedBy = department.CreatedBy,
        CreatedOn = department.CreatedOn,
        IsDeleted = department.IsDeleted,
        Code = department.Code,
        LastModifiedBy = department.LastModifiedBy,
        LastModifiedOn = department.LastModifiedOn
    };
    public static Department ToEntity(this DepartmentRequest departmentRequest) => new()
    {
        Name = departmentRequest.Name,
        Description = departmentRequest.Description,
        Code = departmentRequest.Code,
        CreatedOn = departmentRequest.CreatedOn,
    };
    public static Department ToEntity(this DepartmentUpdateRequest departmentRequest) => new()
    {
        Id = departmentRequest.Id,
        Name = departmentRequest.Name,
        Description = departmentRequest.Description,
        Code = departmentRequest.Code,
    };
    public static DepartmentUpdateRequest ToUpdateRequest(this DepartmentDetailsResponse departmentRequest) => new()
    {
        Id = departmentRequest.Id,
        Name = departmentRequest.Name,
        Description = departmentRequest.Description,
        Code = departmentRequest.Code,
        CreatedOn = DateOnly.FromDateTime(departmentRequest.CreatedOn),
    };


    public static DepartmentRequest ToRequest(this DepartmentUpdateRequest departmentRequest) => new()
    {
        Name = departmentRequest.Name,
        Description = departmentRequest.Description,
        Code = departmentRequest.Code,
    };
}
