using Newtonsoft.Json;
using System.Text;

namespace App.Infra.Integration.RabbitMq.Extensions
{
    internal static class DynamicExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string Serialize<TMessage>(this TMessage message)
            => JsonConvert.SerializeObject(message);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static byte[] GetBytes(this string body)
            => Encoding.UTF8.GetBytes(body);        
    }
}
