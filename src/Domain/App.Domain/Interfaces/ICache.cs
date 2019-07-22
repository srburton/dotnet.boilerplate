using System;

namespace App.Domain.Interfaces
{
    public interface ICacheType { }

    public struct SessionCache: ICacheType
    {
        /// <summary>
        /// User Identification.
        /// </summary>
        public string Sid { get; set; }
        /// <summary>
        /// Group values types
        /// </summary>
        public string Name { get; set; }
        
        public SessionCache(string sid, string name)
        {
            Sid = sid;
            Name = name;            
        }
    }

    public struct PublicCache : ICacheType
    {
        /// <summary>
        /// 
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Action { get; set; }

        public PublicCache(string controller, string action)
        {
            Controller = controller;
            Action = action;
        }
    }

    public struct PrivateCache : ICacheType
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        public PrivateCache(string key)
        {
            Key = key;
        }
    }

    public interface ICache<Service>
    {
        void Set<IType>(IType option, object value, TimeSpan? expiration)
            where IType : ICacheType;

        IValue Get<IType, IValue>(IType option, Func<IValue> set)
             where IType : ICacheType;

        IValue Get<IType, IValue>(IType option)
            where IType : ICacheType;

        void Update<IType>(IType option, object value, TimeSpan? expiration)
            where IType : ICacheType;

        bool Delete<IType>(IType option)
            where IType : ICacheType;
    }
}
