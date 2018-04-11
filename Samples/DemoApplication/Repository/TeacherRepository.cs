using DemoApplication.Entities;

namespace DemoApplication.Repository
{
    public class TeacherRepository : DataRepository<Teacher>
    {
        public TeacherRepository()
            : base(ConfigSetting.DBConnectionString)
        {
        }

        public TeacherRepository(string dbConStr)
            : base(dbConStr)
        {
        }
    }
}