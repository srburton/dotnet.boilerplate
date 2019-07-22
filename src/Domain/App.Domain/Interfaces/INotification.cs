namespace App.Domain.Interfaces
{
    public interface INotification<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="body"></param>
        void Send(string to, string body);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="body"></param>
        void Send(string from, string to, string body);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="to"></param>
        /// <param name="body"></param>
        void Send<B>(string to, B body);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="body"></param>
        void Send<B>(string from, string to, B body);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="to"></param>
        /// <param name="body"></param>
        void Send(string[] tos, string body);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="B"></typeparam>
        /// <param name="to"></param>
        /// <param name="body"></param>
        void Send<B>(string[] tos, B body);
    }
}
