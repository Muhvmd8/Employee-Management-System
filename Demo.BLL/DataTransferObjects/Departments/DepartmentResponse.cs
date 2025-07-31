using System.ComponentModel.DataAnnotations;

namespace Demo.BLL.DataTransferObjects.Departments;
public class DepartmentResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Code { get; set; } = string.Empty;
    [Display(Name = "Creation Date")]
    public DateOnly CreatedOn { get; set; }
}
