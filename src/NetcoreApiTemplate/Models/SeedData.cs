using Microsoft.EntityFrameworkCore;
using NetcoreApiTemplate.Data.Context;
using NetcoreApiTemplate.Models.Table;

namespace NetcoreApiTemplate.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MyDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MyDbContext>>()))
            {
                // Look for any movies.
                if (context.Employees.Any())
                {
                    return;   // DB has been seeded
                }
                context.Employees.AddRange(
                    new Employee
                    {
                        FirstName = "Art",
                        LastName = "MacGyver",
                        Department = "Sport",
                        DateOfBirth = DateTime.Parse("1962-07-03T20:50:25.874Z"),
                    },
                    new Employee
                    {
                        FirstName = "Bennie",
                        LastName = "Weissnat",
                        Department = "Shoes",
                        DateOfBirth = DateTime.Parse("1949-02-26T09:49:13.808Z"),
                    }
                );
                context.SaveChanges();
            }
        }
    }

}
