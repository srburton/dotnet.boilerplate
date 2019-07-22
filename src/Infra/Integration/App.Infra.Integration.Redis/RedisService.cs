using System;
using Newtonsoft.Json;
using App.Infra.Bootstrap;
using StackExchange.Redis;
using App.Domain.Interfaces;
using App.Infra.Bootstrap.Attributes;
using App.Infra.Integration.Redis.Models;
using Microsoft.Extensions.Configuration;
using App.Infra.Integration.Redis.Extensions;

namespace App.Infra.Integration.Redis
{
    /// <summary>
    /// https://stackexchange.github.io/StackExchange.Redis/Basics
    /// </summary>
    [Singleton]
    public class RedisService : IService<RedisService>, ICache<RedisService>
    {
        readonly ConnectionMultiplexer _redis;

        readonly IDatabase _database;

        readonly Option _option;

        public RedisService(IConfiguration configuration)
        {
            _option = Option.Parse(configuration);

            _redis = ConnectionMultiplexer.Connect(_option.HostConnection);

            _database = (_option.IsCluster) ? _redis.GetDatabase() : _redis.GetDatabase(_option.DatabaseDefault);
        }

        public IValue Get<IType, IValue>(IType option, Func<IValue> set) where IType : ICacheType
        {
            try
            {
                var payload = Deserialize(_database.StringGet(MakeKey(option)));

                if (payload == null || payload.Data == null || DateTime.Now >= payload.Expiration)
                    throw new Exception();

                return payload.Data.ToComplexType<IValue>();
            }
            catch (Exception)
            {
                return set();
            }            
        }

        public IValue Get<IType, IValue>(IType option) where IType : ICacheType
            => Get<IType, IValue>(option, () => throw new RedisException($"No values for the key {MakeKey(option)}"));

        public void Set<IType>(IType option, object value, TimeSpan? expiration) where IType : ICacheType
            => _database.StringSet(MakeKey(option),Serialize(value, expiration));

        public void Update<IType>(IType option, object value, TimeSpan? expiration) where IType : ICacheType
        {
            if (_database.KeyExists(MakeKey(option)))
            {
                Delete(option);

                Set(option, value, expiration);
            }
        }

        public bool Delete<IType>(IType option) where IType : ICacheType
           => _database.KeyDelete(MakeKey(option));

        private string MakeKey(ICacheType option)
        {
            switch (option)
            {
                case SessionCache ses:
                    return $"session:{ses.Sid.Clean()}:{ses.Name.Clean()}:";
                case PublicCache pub:
                    return $"public:{pub.Controller.Clean()}:{pub.Action.Clean()}:";
                case PrivateCache pri:
                    return $"provate:{pri.Key.Clean()}:";
                default:
                    throw new InvalidCastException($"Type {nameof(option)} not is {nameof(ICacheType)}.");
            }
        }

        private string Serialize(object obj, TimeSpan? expiration)
        {            
            return JsonConvert.SerializeObject(new Payload()
            {
                Data = obj,
                Expiration = DateTime.Now.AddSeconds(expiration?.TotalSeconds ?? _option.MinutesExpireDefault)
            });
        }

        private Payload Deserialize(string str)
        {
            try
            {
                return JsonConvert.DeserializeObject<Payload>(str);
            }
            catch (Exception)
            {
                return default(Payload);
            }
        }
    }
}
