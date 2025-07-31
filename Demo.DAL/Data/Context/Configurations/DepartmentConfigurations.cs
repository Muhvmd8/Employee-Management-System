namespace Demo.DAL.Data.Context.Configurations;
internal class DepartmentConfigurations : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.Property(d => d.Id)
            .UseIdentityColumn(10, 10);

        builder.Property(d => d.Name)
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired(true);

        builder.Property(d => d.CreatedOn)
            .HasDefaultValueSql("GETUTCDATE()");

    }
}
