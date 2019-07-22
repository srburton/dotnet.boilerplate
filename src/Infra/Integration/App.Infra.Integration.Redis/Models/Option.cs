using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace App.Infra.Integration.Redis.Models
{
    internal class Option
    {
        /// <summary>
        /// Example:
        /// [
        ///   "server:port",
        ///   "server1:6379",
        ///   "server2:6379"
        /// ]
        /// </summary>
        public string[] Hosts { get; set; }  
        /// <summary>
        /// Use index database.
        /// </summary>
        public int? Database { get; set; }
        /// <summary>
        /// Minutes to expire
        /// </summary>
        public double? MinutesExpire { get; set; }

        /// <summary>
        /// Note that Redis supports multiple databases (although this is not supported in "cluster");
        /// </summary>
        public string HostConnection
          => (Database == null) ? string.Join(",", Hosts) : Hosts.ToList().FirstOrDefault();

        public bool IsCluster
          => (Database == null && Hosts.Count() > 1);

        public double MinutesExpireDefault
          => TimeSpan.FromMinutes(MinutesExpire ?? 10).TotalMinutes;

        public int DatabaseDefault
          => (IsCluster) ? -1 : Database ?? -1;

        public static Option Parse(IConfiguration configuration)
          => configuration.GetSection(nameof(Redis))
                            .Get<Option>();
    }
} 