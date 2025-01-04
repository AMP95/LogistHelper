using Shared;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq.Expressions;
using Utilities;

namespace DAL
{
    public class LogistRepository : ICrudRepository
    {
        private ILogger _logger;
        private DbContext _context;

        public LogistRepository(ILogger logger, DbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public virtual IEnumerable<T> Get<T>(Expression<Func<T, bool>> filter = null,
                                             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                             string includeProperties = "") where T : class
        {
            if (_context != null)
            {
                IQueryable<T> query = _context.Set<T>();
                if (query.Any())
                {
                    if (filter != null)
                    {
                        query = query.Where(filter);
                    }

                    foreach (string includeProperty in includeProperties.Split
                        (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }

                    if (orderBy != null)
                    {
                        return orderBy(query).ToList();
                    }
                    else
                    {
                        return query.ToList();
                    }
                }
            }
            return Enumerable.Empty<T>();
        }

        public virtual T Get<T>(object id) where T : class
        {
            if (_context != null)
            {
                return _context.Set<T>().Find(id);
            }
            return default;
        }

        public virtual bool Update<T>(T entity) where T : class
        {
            if (_context != null)
            {
                using (DbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Set<T>().AddOrUpdate(entity);
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.Log(ex, $"Error update entity {typeof(T).Name} " + ex.Message, LogLevel.Error);
                    }
                }
            }
            return false;
        }

        public virtual bool UpdateRange<T>(IEnumerable<T> entitiesToUpdate) where T : class
        {
            if (_context != null)
            {
                using (DbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (T entity in entitiesToUpdate)
                        {
                            _context.Set<T>().AddOrUpdate(entity);
                        }
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.Log(ex, $"Error update entity {typeof(T).Name} " + ex.Message, LogLevel.Error);
                    }
                }
            }
            return false;
        }

        public virtual bool Remove<T>(object id) where T : class
        {
            if (_context != null)
            {
                using (DbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        T entity = _context.Set<T>().Find(id);
                        _context.Set<T>().Remove(entity);
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.Log(ex, $"Error remove entity {typeof(T).Name} " + ex.Message, LogLevel.Error);
                    }
                }
            }
            return false;
        }

        public virtual bool Remove<T>(T entityToDelete) where T : class
        {
            if (_context != null)
            {
                using (DbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Set<T>().Remove(entityToDelete);
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.Log(ex, $"Error remove entity {typeof(T).Name} " + ex.Message, LogLevel.Error);
                    }
                }
            }
            return false;
        }

        public virtual bool RemoveRange<T>(IEnumerable<T> entitiesToDelete) where T : class
        {
            if (_context != null)
            {
                using (DbContextTransaction transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Set<T>().RemoveRange(entitiesToDelete);
                        _context.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.Log(ex, $"Error remove entity {typeof(T).Name} " + ex.Message, LogLevel.Error);
                    }
                }
            }
            return false;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
