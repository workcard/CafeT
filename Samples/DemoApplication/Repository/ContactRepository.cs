using DemoApplication.Entities;

namespace DemoApplication.Repository
{
    public class ContactRepository : DataRepository<Contact>
    {
        public ContactRepository()
            : base(ConfigSetting.DBConnectionString)
        {
        }

        public ContactRepository(string dbConStr)
            : base(dbConStr)
        {
        }
    }
}