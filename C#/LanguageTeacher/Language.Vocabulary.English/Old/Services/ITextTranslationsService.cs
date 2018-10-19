using Utils.Collections;

namespace Language.Vocabulary.Core {
    public interface ITextTranslationsService<T> {
        ICollection<T> GetTranslations(IUnit text);
    }
}
