using Utils.Collection;

namespace Language.Vocabulary.Core {
    public interface ITextTranslationsService<T> {
        ICollection<T> GetTranslations(IText text);
    }
}
