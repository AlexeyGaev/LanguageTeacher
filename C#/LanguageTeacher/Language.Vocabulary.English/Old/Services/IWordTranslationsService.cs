using Utils.Collections;

namespace Language.Vocabulary.Core {
    public interface IWordTranslationsService<T> {
        ICollection<T> GetTranslations(IContextUnit word);
    }
}
