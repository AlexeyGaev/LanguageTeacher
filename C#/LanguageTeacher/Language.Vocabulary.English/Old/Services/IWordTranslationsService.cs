using Utils.Collections;

namespace Language.Vocabulary.Core {
    public interface IWordTranslationsService<T> {
        IEnumerator<T> GetTranslations(IContextUnit word);
    }
}
