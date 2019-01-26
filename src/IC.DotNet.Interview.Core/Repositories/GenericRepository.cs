using IC.DotNet.Interview.Core.Database;
using IC.DotNet.Interview.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace IC.DotNet.Interview.Core.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly IDbContext _dbContext;
        private readonly ICollection<T> _dataset;


        public GenericRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _dataset = _dbContext.GetDataset<T>();
        }

        public ICollection<T> Get()
        {
            return _dataset;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> expression)
        {
            return _dataset.AsQueryable().Where(expression);
        }

        public T Get(Guid id)
        {
            return _dataset.FirstOrDefault(e => e.Id.Equals(id));
        }

        public bool Create(T model)
        {
            if (model == null)
                return false;

            model.Id = Guid.NewGuid();

            _dataset.Add(model);
            _dbContext.Save();
            return true;
        }

        public bool Update(T model) // removed the ID it isn't necessary as ID is not a variable we will be able to change
        {
            T oldModel = Get(model.Id); // get the old unaltered object
            if (model == null || oldModel == null) // check if the model is not empty and if it already exists in the DB (can't edit a non existing entity)
                return false;

            if (!_dataset.Remove(Get(model.Id))) //changed this since the model variable is the updated version and we need to remove the old one
                return false;
            
            _dataset.Add(model);    // we can use oldModel = model;
            _dbContext.Save();
            return true;
        }

        public bool Delete(Guid id)
        {
            T model = Get(id);
            if (model == null)
                return false;

            if (!_dataset.Remove(model))
                return false;

            _dbContext.Save();
            return true;
        }
    }
}
