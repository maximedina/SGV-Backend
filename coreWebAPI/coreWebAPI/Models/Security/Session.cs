using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MOM.Core.Models.Security
{
    public class Session
    {
        public long Id { get; set; }

        public User User {get;set;}

        public string Token { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastAction {get; set;}

        public string DeviceType { get; set; }

        [NotMapped]
        public List<Permission> Permissions { get; set; } = new List<Permission>();
    }
}