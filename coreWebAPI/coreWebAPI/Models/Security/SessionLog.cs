using System;

namespace MOM.Core.Models.Security
{
    public class SessionLog
    {
        public long Id { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastAction { get; set; }

        public string DeviceType { get; set; }

        public static explicit operator SessionLog(Session v)
        {
            var session = new SessionLog()
            {
                CreationTime = v.CreationTime,
                DeviceType = v.DeviceType,
                LastAction = v.LastAction,
                Token = v.Token,
                UserId = (v.User != null ) ? v.User.UserId : 0
            };

            return session;
        }
    }
}