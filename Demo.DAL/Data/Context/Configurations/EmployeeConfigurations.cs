
namespace Demo.DAL.Data.Context.Configurations;
internal class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(e => e.Salary)
            .HasColumnType("decimal(10,2)");


        builder.Property(e => e.Gender)
            .HasConversion(g => g.ToString(),
            g => Enum.Parse<Gender>(g)
            );
        builder.HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull);


    }
}
