namespace Demo.DAL.Models;
public class Department : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<Employee> Employees { get; set; } = []; // 12 C# []
}
