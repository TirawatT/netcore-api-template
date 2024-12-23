using Dapper;
using NetcoreApiTemplate.Data.Context;

namespace NetcoreApiTemplate.Data.Repositorys
{
    public class CommonRepository
    {
        private readonly MyDbContext _db;

        public CommonRepository(MyDbContext db)
        {
            _db = db;
        }
        public string GetDbInfo()
        {
            var sql = "SELECT HOST_NAME ||  '-' || VERSION AS DBNAME FROM V$INSTANCE";
            var result = _db.Connection.Query<string>(sql).FirstOrDefault() ?? string.Empty;
            return result;
        }
        public DateTime GetSysDate()
        {
            var sql = "SELECT SYSDATE FROM dual";
            var res = _db.Connection.Query<DateTime>(sql).FirstOrDefault();
            return res;
        }

    }
}
