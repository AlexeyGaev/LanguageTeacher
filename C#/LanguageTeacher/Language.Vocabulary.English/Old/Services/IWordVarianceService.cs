using Utils.Collections;

namespace Language.Vocabulary.Core {
    public interface IWordVarianceService<T> {
        ICollection<T> GetWords(IContextUnit unit);
    }
}
