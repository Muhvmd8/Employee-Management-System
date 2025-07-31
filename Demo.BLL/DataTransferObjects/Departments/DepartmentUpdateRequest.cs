namespace Demo.BLL.DataTransferObjects.Departments;
public class DepartmentUpdateRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateOnly CreatedOn { get; set; }
}
