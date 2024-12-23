using NetcoreApiTemplate.Data.Context;
using NetcoreApiTemplate.Data.Repositorys.Base;
using NetcoreApiTemplate.Models.Table;

namespace NetcoreApiTemplate.Data.Repositorys
{
    public class EmployeeRepository(MyDbContext context) : Repository<Employee, MyDbContext>(context)
    {

        public IEnumerable<Employee> GetAll(string department = "")
        {
            var res = base.GetAll();
            if (!string.IsNullOrEmpty(department))
                res = res.Where(w => w.Department == department);
            res = res.Take(5);
            return res;
        }
    }
}
