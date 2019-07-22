using System;

namespace App.Domain.Interfaces
{
    public interface ILog<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exp"></param>
        void Fatal(Exception exp);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="err"></param>
        void Error(string err);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="war"></param>
        void Warning(string war);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inf"></param>
        void Information(string inf);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deb"></param>
        void Debug(string title, object deb, Exception exc = null);
    }
}
