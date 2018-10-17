using Utils.Collection;

namespace LanguageVocabulary.Core {
    public interface IVocabularyItems<T> {
        IUnit Description { get; }
        ICollection<T> Items { get; }
    }
}
