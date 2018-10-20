namespace Language.Common.Utils {
    public interface IEnumerableService<T> {
        IEnumerator<T> GetEnumerator();
    }
}
