namespace Demo.BLL.DataTransferObjects.Departments;
public class DepartmentRequest
{
    [Required(ErrorMessage = "Name Is Required !!??")]
    [StringLength(maximumLength: 50, MinimumLength = 5)]
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    //[MinLength(10 )]
    public string? Description { get; set; }
    public DateTime CreatedOn { get; set; }
}
