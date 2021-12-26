using MOM.Core.Db;
using System.Collections.Generic;
using System.Linq;

namespace MOM.Core.Models.Shared
{
    public class Uom
    {
        public int UomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        private CoreDbContext _dbContext;

        public Uom(CoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Uom> List() => _dbContext.Uoms.ToList();
    }
}