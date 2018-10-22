namespace Language.Common.Utils {
    public interface IKeysEnumeratorService<K> {
        IEnumerator<K> Keys { get; }
    }
}
