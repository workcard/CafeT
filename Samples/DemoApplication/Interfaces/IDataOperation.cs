using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace DemoApplication.Interfaces
{
    public interface IDataOperation<T>
    {
        T GetByGUID(string guid);
        object Add(T entity);
        void Delete(string guid);
        T Update(T entity);
        IList<T> GetAll();
        IList<T> GetEntityByParams(SqlParameter[] sqlParams, string spName);
    }
}
