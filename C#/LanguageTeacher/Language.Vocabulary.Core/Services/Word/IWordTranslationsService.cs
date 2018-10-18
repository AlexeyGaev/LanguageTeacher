using Utils.Collection;

namespace Language.Vocabulary.Core {
    public interface IWordTranslationsService<T> {
        ICollection<T> GetTranslations(IWord word);
    }
}
