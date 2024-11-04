using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;

namespace test1.Interfaces
{
    public interface IRepo<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        bool Exists(Func<TEntity, bool> predicate);
        TEntity Create(TEntity entity);
        TEntity GetByCondition(Func<TEntity, bool> predicate);
        List<TEntity> GetListByCondition(Func<TEntity, bool> predicate);
        
        bool Save();  
        public void Update(TEntity entity);
        public void Delete(TEntity entity);
        public void Add(TEntity entity) ;
        public TEntity GetByCondition(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>> include = null);

        public List<TEntity> GetListByCondition(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> include = null);


    }
}
