using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MOM.Core.Util.Extensions
{
    public static class EntityExtensions
    {
        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> source, IEnumerable<string> navigationPropertyPaths) where T : class
        {
            return navigationPropertyPaths.Aggregate(source, (query, path) => query.Include(path));
        }
    }
}