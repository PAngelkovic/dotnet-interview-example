using IC.DotNet.Interview.Core.Database;
using IC.DotNet.Interview.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

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
            PopulateHistoryData(model);

            _dataset.Add(model);
            _dbContext.Save();
            return true;
        }

        public bool Update(T model) // removed the ID it isn't necessary as ID is not a variable we will be able to change
        {
            if (model == null) 
                return false;

            if (!_dataset.Remove(model))  
                return false;
            PopulateHistoryData(model, false);

            _dataset.Add(model);    
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

        private T PopulateHistoryData(T model, bool isNew = true) {
            var userId = HttpContext.Current.Request.Cookies["CurrentUser"];
            if (userId != null)
            {
                var userGuid = new Guid(userId.Value);
                if (isNew)
                {
                    model.UserCreatedId = userGuid;
                    model.DateCreated = DateTime.Now; //In a real implementation we should take into consideration locale if we wish to display data in countries in different time zones
                }
                model.UserLastUpdatedId = userGuid;
                model.LastUpdated = DateTime.Now;
            }
            else {
                //generally this shouldn't happen since if there isn't a logged in user he shouldn't get this far but just in case
                if (isNew) {
                    model.UserCreatedId = Guid.Empty;
                    model.DateCreated = DateTime.Now;
                }
                model.UserLastUpdatedId = Guid.Empty;
                model.LastUpdated = DateTime.Now;
            }
            return model;
        }
    }
}
