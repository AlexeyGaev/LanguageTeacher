using Utils.Collections;

namespace Utils.Collections.Services {
    public interface IEnumerableService<T> {
        IEnumerator<T> GetEnumerator();
    }
}
