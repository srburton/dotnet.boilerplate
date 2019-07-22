using System;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace App.Infra.Integration.RabbitMq.Modules
{
    public class ConnectionConfigurationBuild
    {
        private ConnectionConfiguration Configuration { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public ConnectionConfigurationBuild(ConnectionConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        public void ClientProvidedAssembly<Type>()
        {
            string assemblyName = typeof(Type).GetTypeInfo().Assembly.GetName().Name;
            Configuration.ClientProvidedName = assemblyName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="automaticRecovery"></param>
        /// <param name="maxRetryCount"></param>
        /// <param name="maxRetryDelay"></param>
        public void EnableRetryOnFailure(bool automaticRecovery, int maxRetryCount, TimeSpan maxRetryDelay)
        {
            Configuration.AutomaticRecoveryEnabled = automaticRecovery;
            Configuration.FailReConnectRetryCount = maxRetryCount;
            Configuration.NetworkRecoveryInterval = maxRetryDelay;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxRetryDelay"></param>
        public void RetryOnFailure(TimeSpan maxRetryDelay)
        {
            Configuration.ConsumerFailRetryInterval = maxRetryDelay;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        public void LoggingWriteLevel(LogLevel level)
        {
            Configuration.Level = level;
        }
    }
}
