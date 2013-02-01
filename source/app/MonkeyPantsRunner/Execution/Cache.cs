using System.Collections.Generic;

namespace MonkeyPants.Execution
{
    public class Cache<T>
    {
        private readonly Dictionary<T, object> cache = new Dictionary<T, object>();

        public object this[T key]
        {
            get
            {
                object result;
                cache.TryGetValue(key, out result);
                return result;
            }
            set { cache[key] = value; }
        }
    }
}