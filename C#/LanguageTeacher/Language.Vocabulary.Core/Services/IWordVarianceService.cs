using Utils.Collections;

namespace Language.Vocabulary.Core {
    public interface IWordVarianceService<T> {
        IEnumerator<T> GetWords(IUnit unit);
    }
}
