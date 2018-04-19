using CafeT.Text;
using CafeT.Translators;
using Mvc5.CafeT.vn.Models;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mvc5.CafeT.vn.Managers
{
    public class WordManager : ObjectManager
    {
        public WordManager(IUnitOfWorkAsync unitOfWorkAsync) : base(unitOfWorkAsync)
        {
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public WordModel GetById(Guid id)
        {
            var _object = _unitOfWorkAsync.Repository<WordModel>().Find(id);
            return _object;
        }

        public IEnumerable<WordModel> GetNews(string userName)
        {
            if(!userName.IsNullOrEmptyOrWhiteSpace())
            {
                var models = GetAll()
                  .Where(t => t.IsOf(userName))
                  .Where(t => t.IsRemembered == false)
                  .AsEnumerable();
                return models;
            }
            return null;
        }

        public bool IsExits(string word)
        {
            var _models = _unitOfWorkAsync.Repository<WordModel>().Query()
                .Select(t => t.Model.Value);
            if (_models.Contains(word)) return true;
            return false;
        }
        
        public void Update(WordModel model)
        {
            _unitOfWorkAsync.Repository<WordModel>().Update(model);
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool Insert(WordModel model)
        {
            if(!IsExits(model.Model.Value))
            {
                Translator translator = new Translator();
                //model.Translation = translator.Trans(model.Model.Value);
                model.Translation = translator.Translate(model.Model.Value);
                
                if (model.Translation != string.Empty)
                {
                    _unitOfWorkAsync.RepositoryAsync<WordModel>().Insert(model);
                    try
                    {
                        _unitOfWorkAsync.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return false;
        }

        public IEnumerable<WordModel> GetAll()
        {
            var _models = _unitOfWorkAsync.RepositoryAsync<WordModel>().Query()
                            .Select();

            return _models.AsEnumerable();
        }
    }
}