using Utils.Collection;

namespace LanguageVocabulary.Core {
    public interface IVocabularyUnit<T> {
        IUnit Description { get; }
        ICollection<T> Items { get; }
    }
}
