using Utils.Collection;

namespace Language.Vocabulary.Core {
    public interface IService<T, U> {
        T GetInfo(U unit);
    }
}
