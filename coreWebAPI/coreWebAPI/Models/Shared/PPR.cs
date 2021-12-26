using System;
using System.Collections.Generic;

namespace MOM.Core.Models.Shared
{
    public class PPR
    {
        public int PPRId { get; set; }
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public string PprName { get; set; }
        public string PprDescription { get; set; }
        public int? LocationId { get; set; }
        public char Released { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? BillOfMaterialsId { get; set; }
        public int? BillOfResourcesId { get; set; }
    }
}