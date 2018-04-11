using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MathBot.Models;

namespace MathBot.Managers
{
    [Serializable]
    public class CodeManager
    {
        private MathBotDataContext db = new MathBotDataContext();


        public CodeFunction GetFunctionByName(string name)
        {
            return db.CodeFunctions.Where(t => t.Name.ToLower().Trim() == name.ToLower().Trim()).FirstOrDefault();
        }
        public IEnumerable<CodeFunction> GetAll()
        {
            return db.CodeFunctions.AsEnumerable();
        }
        public CodeFunction GetFunctionById(Guid id)
        {
            return db.CodeFunctions.Where(t => t.Id == id).FirstOrDefault();
        }

        public async System.Threading.Tasks.Task<bool> AddFunctionAsync (CodeFunction model)
        {
            var _functions = db.CodeFunctions.Select(t => t.Name.ToLower());
            if(!_functions.Contains(model.Name.ToLower()))
            {
                db.CodeFunctions.Add(model);
                var _value = await db.SaveChangesAsync();
                if (_value >= 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}