using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace MOM.Core.Db.Util
{
    public static class AddOrUpdateExtension
    {
        public static List<EntityEntry<TEntity>> AddOrUpdate<TEntity>(this DbSet<TEntity> dbSet, ref List<TEntity> entities, Expression<Func<TEntity, object>> expression)
            where TEntity : class
        {
            var entries = new List<EntityEntry<TEntity>>();
            
            for (int i = 0; i < entities.Count(); i++)
            {
                var entity = entities[i];

                entries.Add(dbSet.AddOrUpdate(ref entity, expression));

                entities[i] = entity;
            }

            return entries;
        }

        public static EntityEntry<TEntity> AddOrUpdate<TEntity>(this DbSet<TEntity> dbSet, ref TEntity entity, Expression<Func<TEntity, object>> expression)
            where TEntity : class
        {

            if (EntityExists(dbSet, expression, ref entity))
            {
                return dbSet.GetContext().Update(entity);
            }
            else
            {
                return dbSet.GetContext().Add(entity);
            }
        }

        /// <summary>
        /// Checks dynamically if the entity exists in the databse. If it does, changes the referenced entity to the database entity, plus with it's values changed using the original entity.
        /// If the entity exists, won't change primary keys, navigation properties, properties with private getters and foreign keys ids in wich the given values are equal to 0.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="context"></param>
        /// <param name="expression"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static bool EntityExists<TEntity>(DbSet<TEntity> dbSet, Expression<Func<TEntity, object>> expression, ref TEntity entity)
            where TEntity : class
        {
            var _context = dbSet.GetContext();

            var t = typeof(TEntity);

            var filterProperties =
                expression.Compile()(entity)
                .GetType().GetProperties().Select(p => t.GetProperty(p.Name)).ToList();

            if (filterProperties == null || filterProperties.Count == 0)
            {
                throw new Exception($"{t.FullName} does not have a KeyAttribute field. Unable to exec AddOrUpdate call.");
            }

            int? ct = null;

            int GenerateIndex()
            {
                if (ct == null)
                {
                    ct = 0;
                    return ct.Value;
                }
                else
                {
                    ct++;
                }

                return ct.Value;
            }

            var _entity = entity;

            var namesAndValuesToFilter = filterProperties
                .Select(x => new
                {
                    Index = GenerateIndex(),
                    x.Name,
                    Value = x.GetValue(_entity)
                })
                .ToList();

            var expressions = string.Join(" AND ", namesAndValuesToFilter.OrderBy(x => x.Index).Select(x => $"{x.Name} == @{x.Index}"));

            var values = namesAndValuesToFilter.OrderBy(x => x.Index).Select(x => x.Value).ToArray();

            var entityType = _context.Model.FindEntityType(typeof(TEntity));

            var pkKeyName = entityType.FindPrimaryKey().Properties
                .Select(x => x.Name).FirstOrDefault() ?? "Id";

            var queryResult = _context.Set<TEntity>().AsNoTracking()
                .Where(expressions, values)
                .Select($"new ({pkKeyName})")
                .FirstOrDefault();

            if (queryResult != null)
            {
                // Gets the original Entity from the database and change only simple fields.
                var pksAndNavigations = entityType
                    .FindPrimaryKey().Properties.Select(x => x.Name).Distinct().ToList();

                var fks = entityType
                    .GetForeignKeys().SelectMany(x => x.Properties.Select(p => p.Name)).Distinct().ToList();

                pksAndNavigations.AddRange(entityType
                    .GetNavigations().Select(x => x.Name).Distinct().ToList());

                var propsToChange = t.GetProperties()
                    .Where(x => !pksAndNavigations.Contains(x.Name) && x.CanWrite)
                    .ToList();

                var fullEntity = _context.Set<TEntity>()
                    .Where(expressions, values)
                    .FirstOrDefault();

                var dynamicChangeableFields = new ExpandoObject() as IDictionary<string, object>;

                foreach (var prop in propsToChange)
                {
                    var value = t.GetProperty(prop.Name).GetValue(entity, null);

                    // Allow to change only not null or FK values only if they are not 0.
                    if (value == null || (fks.Contains(prop.Name) && value is int && Convert.ToInt32(value) == 0))
                    {
                        continue;
                    }

                    dynamicChangeableFields.Add(prop.Name, value);
                }

                _context.Entry(fullEntity).CurrentValues.SetValues(dynamicChangeableFields as ExpandoObject);
                _context.Entry(fullEntity).State = EntityState.Modified;

                entity = fullEntity;

                return true;
            }
            else
            {
                return false;
            }
        }
        
        private static DbContext GetContext<TEntity>(this DbSet<TEntity> dbSet) where TEntity : class
        {
            return (DbContext)dbSet
                .GetType().GetTypeInfo()
                .GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(dbSet);
        }
    }
}
