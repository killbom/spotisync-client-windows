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
        private static Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void RegisterService<TKey, TService>()
        {
            var instance = Activator.CreateInstance(typeof(TService));
            _services.Add(typeof(TKey), instance);
        }

        public static T GetService<T>() where T : class
        {
            if (_services.ContainsKey(typeof(T))) 
            {
                var service = _services[typeof(T)];
                if (service == null)
                {
                    var instance = Activator.CreateInstance(typeof(T));
                    _services[typeof(T)] = instance;

                    return instance as T;
                }
                else
                {
                    return service as T;
                }

            } else
            {
                throw new KeyNotFoundException($"{typeof(T)} is not registered");
            }
        }
    }
}
