using MOM.Core.Db;
using System.Collections.Generic;
using System.Linq;

namespace MOM.Core.Models.Shared
{
    public class Shape
    {
        public int ShapeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        private CoreDbContext _dbContext;

        public Shape(CoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Shape> List() => _dbContext.Shapes.ToList();
    }
}