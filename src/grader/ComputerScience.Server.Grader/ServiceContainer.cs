using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerScience.Server.Grader
{
    public class ServiceContainer
    {
        private IDictionary<Type, Service> Dictionary { get; }
        private IDictionary<Type, object> Existing { get; }

        public ServiceContainer()
        {
            Dictionary = new Dictionary<Type, Service>();
            Existing = new Dictionary<Type, object>();
        }

        public void Add<T>(Func<ServiceContainer, object> service)
        {
            Dictionary.Add(typeof(T), new Service(service));
        }

        public T GetExisting<T>() where T : class
        {
            if (Existing.ContainsKey(typeof(T)))
                return (T) Existing[typeof(T)];
            throw new KeyNotFoundException();
        }

        public T CreateNew<T>()
        {
            var service = Dictionary[typeof(T)];
            if (service == null)
                throw new UnauthorizedAccessException();
            return (T) service.Run(this);
        }
    }
}
