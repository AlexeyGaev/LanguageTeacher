using Utils.Collection;

namespace Utils.Scanner {
    public interface IScanner<S, T> {
        ICollection<T> Scan(S source);
    }
}
