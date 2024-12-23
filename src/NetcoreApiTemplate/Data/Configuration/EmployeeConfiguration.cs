
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetcoreApiTemplate.Models.Table;

namespace NetcoreApiTemplate.Data.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("EMPLOYEE");
            builder.HasKey(t => new { t.Id });
            builder.Property(t => t.Id).HasColumnName("ID").HasColumnType("Int64");
            builder.Property(t => t.FirstName).HasColumnName("FIRST_NAME").HasColumnType("Varchar2");
            builder.Property(t => t.LastName).HasColumnName("LAST_NAME").HasColumnType("Varchar2");
            builder.Property(t => t.Department).HasColumnName("DEPARTMENT").HasColumnType("Varchar2");
            builder.Property(t => t.DateOfBirth).HasColumnName("DATE_OF_BIRTH").HasColumnType("Date");
        }
    }
}
