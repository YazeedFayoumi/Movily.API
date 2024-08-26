using Microsoft.EntityFrameworkCore.Diagnostics;

namespace test1.Interfaces
{
    public interface IRepo<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        bool Exists(Func<TEntity, bool> predicate);
        TEntity Create(TEntity entity);
        TEntity GetByCondition(Func<TEntity, bool> predicate);
        List<TEntity> GetListByCondition<TEntity>(Func<TEntity, bool> predicate) where TEntity : class;
        //List<TEntity> GetListByCondition(Func<TEntity, bool> predicate);
        //List<TEntity> GetListByCondition(int id);
        bool Save();  
        public void Update(TEntity entity);
        public void Delete(TEntity entity);
        public void Add<TEntity>(TEntity entity) where TEntity :class;
       
    }
}
