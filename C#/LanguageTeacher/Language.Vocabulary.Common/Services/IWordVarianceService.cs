using Language.Common.Utils;

namespace Language.Vocabulary.Services {
    public interface IWordVarianceService<T> {
        IEnumerator<T> GetWords(IUnit unit);
    }
}
