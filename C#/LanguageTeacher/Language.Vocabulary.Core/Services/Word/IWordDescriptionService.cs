using Utils.Collection;

namespace Language.Vocabulary.Core {
    public interface IDescriptionService<T> {
        T GetDescription(IWord unit);
    }
}
