using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Context;
using DataLayer.Entities;

namespace CoreLayer.Services
{
   public abstract class BaseService 
    {
        private readonly ConcurrentDictionary<string, object> _entitySets = new ConcurrentDictionary<string, object>();

        protected EchatContext _context { get; private set; }

        protected BaseService(EchatContext context)
        {
            _context = context;
        }


        #region Methods
        protected virtual void Insert<T>(T entity) where T : BaseEntities
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities<T>().Add(entity);
        }

        protected virtual void Insert<T>(IEnumerable<T> entities) where T : BaseEntities
        {
            if (entities?.Any() != true)
            {
                throw new ArgumentException(nameof(entities));
            }

            Entities<T>().AddRange(entities);
        }

        protected virtual void Update<T>(T entity)
            where T : BaseEntities
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = _context.Entry(entity);
            if (entry == null)
            {
                var cachedEntry = _context.ChangeTracker.Entries<T>().FirstOrDefault(x => x.Entity.Id == entity.Id);
                if (cachedEntry != null)
                {
                    cachedEntry.State = EntityState.Detached;
                }

                Entities<T>().Attach(entity);
                entry = _context.Entry(entity);
            }

            entry.State = EntityState.Modified;       
        }


        protected virtual void Update<T>(IEnumerable<T> entities)
            where T : BaseEntities
        {
            if (entities?.Any() != true)
            {
                throw new ArgumentException(nameof(entities));
            }

            Entities<T>().AttachRange(entities);
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }         
        }

        protected virtual void Delete<T>(T entity)
            where T : BaseEntities
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Entities<T>().Remove(entity);
        }


        protected List<T> RawSqlQuery<T>(string query, Func<DbDataReader, T> map)
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.Text;

            _context.Database.OpenConnection();

            using var result = command.ExecuteReader();
            var entities = new List<T>();

            while (result.Read())
            {
                entities.Add(map(result));
            }

            return entities;
        }   
        protected virtual void Delete<T>(IEnumerable<T> entities)
            where T : BaseEntities
        {
            if (entities?.Any() != true)
            {
                throw new ArgumentException(nameof(entities));
            }

            Entities<T>().RemoveRange(entities);
        }


        protected Task<int> Save()
        {
            var result = _context.SaveChangesAsync();

            return result;
        }

        protected virtual IQueryable<T> Table<T>() where T : BaseEntities
        {
            return Entities<T>().AsNoTracking();
        }

        protected virtual IQueryable<T> TableTracking<T>()
            where T : BaseEntities
        {
            return Entities<T>();
        }

        protected virtual DbSet<T> Entities<T>()
            where T : BaseEntities
        {
            return _entitySets.GetOrAdd(typeof(T).FullName, (key) => { return _context.Set<T>(); }) as DbSet<T>;
        }


        protected virtual Task<T> GetById<T>(int entityId) where T : BaseEntities
        {
            return Table<T>().SingleOrDefaultAsync(x => x.Id == entityId);
        }

        protected virtual Task<T> GetById<T>(int entityId, params string[] includs) where T : BaseEntities
        {
            IQueryable<T> table = _context.Set<T>();
            foreach (var inc in includs)
            {
                table = table.Include(inc);
            }

            return table.SingleOrDefaultAsync(x => x.Id == entityId);
        }

        #endregion
    }
}
