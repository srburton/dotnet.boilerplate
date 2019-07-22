using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Infra.Integration.Redis.Extensions
{
    internal static class ObjectExtension
    {
        public static T ToComplexType<T>(this object obj)
        {
            var autoMapper = new MapperConfiguration(cfg => cfg.CreateMap<T, object>());

            var mapper = autoMapper.CreateMapper();

            return mapper.Map<T>(obj);
        }         
    }
}
