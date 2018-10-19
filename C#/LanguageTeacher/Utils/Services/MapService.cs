using Utils.Collections.Map;

namespace Utils.Services {
    public class MapService<S, T> : IService<S, T> {
        readonly Map<S, T> map;

        public MapService(Map<S, T> map) {
            this.map = map;
        }

        public T Func(S item) {
            T result;
            if (this.map.TryGetValue(item, out result))
                return result;
            return default(T);
        }
    }
}
