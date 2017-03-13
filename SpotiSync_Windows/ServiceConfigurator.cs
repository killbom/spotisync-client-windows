using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SpotiSync_Windows
{  
    public static class ServiceConfigurator
    {
        private static Dictionary<Type, Type> _servicesMap = new Dictionary<Type, Type>();
        private static Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        public static void RegisterService<TKey, TService>() where TService : TKey
        {
            _servicesMap.Add(typeof(TKey), typeof(TService));
        }

        public static T GetService<T>() where T : class
        {
            if (!_instances.ContainsKey(typeof(T)) && _servicesMap.ContainsKey(typeof(T))) 
            {
                var type = _servicesMap[typeof(T)];
                var instance = Activator.CreateInstance(type);
                _instances.Add(typeof(T), instance);

                return instance as T;

            }
            else if(_instances.ContainsKey(typeof(T)))
            {
                return _instances[typeof(T)] as T;
            }
            else
            {
                throw new KeyNotFoundException($"{typeof(T)} is not registered");
            }
        }
    }
}
