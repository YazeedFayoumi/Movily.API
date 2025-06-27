
using Microsoft.EntityFrameworkCore;
using Nest;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using test1.Data;
using test1.Interfaces;
using test1.Models;

namespace test1.Repositories
{
    public class GenericRepository<TEntity> : IRepo<TEntity> where TEntity : class
    {
        private readonly ClassContextDb _context;
        private DbSet<TEntity> _dbSet;

        public GenericRepository(ClassContextDb context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }
        public TEntity Get(int id)
        {
            return _dbSet.Find(id);
        }

        public bool Exists(Func<TEntity, bool> predicate)
        {

            return _dbSet.Any(predicate);

        }

        public TEntity Create(TEntity entity)
        {
            _dbSet.Add(entity);
            Save();
            return entity;
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved >0 ? true : false;
        }

        /*public TEntity GetByCondition(Func<TEntity, bool> predicate) where TEntity : class
        {
           
        }*/

        public List<TEntity> GetListByCondition(Func<TEntity, bool> predicate) 
        {
            return _dbSet.Where(predicate).ToList();
        }

        /*public List<TEntity> GetListByCondition(Func<TEntity, bool> predicate)
        {
            //return _dbSet.Where(predicate).ToList();
            return _dbSet.AsEnumerable().Where(predicate).ToList();
        }*/


        /* public void Add(TEntity entity)
         {
             _dbSet.Add(entity);
             Save();
         }

         public void Delete(TEntity entity)
         {
             _dbSet.Remove(entity);
             Save();
         }

         public void Update(TEntity entity)
         {
             _dbSet.Attach(entity);
             _context.Entry(entity).State = EntityState.Modified;
             Save();
         }*/

        /* public void Add(TEntity entity) 
         {
             _dbSet.Add(entity);
             Save();
         }*/
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
            Save();
        }

        public void Delete(TEntity entity) 
        {
            _dbSet.Remove(entity);
            Save();
        }

        public void Update(TEntity entity) 
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            Save();
        }

        public TEntity GetByCondition(Func<TEntity, bool> predicate) 
        {
            
            return _context.Set<TEntity>().FirstOrDefault(predicate);
           
        }

        public TEntity GetByCondition(
         Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, object>> include = null) 
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            
            if (include != null)
            {
                query = query.Include(include);
            }

            
            return query.FirstOrDefault(predicate);
        }

        public List<TEntity> GetListByCondition(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> include = null) 
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            
            if (include != null)
            {
                query = query.Include(include);
            }

            
            return query.Where(predicate).ToList();
        }
    }
}



