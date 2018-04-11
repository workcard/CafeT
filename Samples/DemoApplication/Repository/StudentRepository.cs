using DemoApplication.Entities;

namespace DemoApplication.Repository
{
    public class StudentRepository : DataRepository<Student>
    {
        public StudentRepository()
            : base(ConfigSetting.DBConnectionString)
        {
        }

        public StudentRepository(string dbConStr)
            : base(dbConStr)
        {
        }
    }
}