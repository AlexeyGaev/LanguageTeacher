using Utils.Collections;

namespace Language.Vocabulary.Core {
    public interface ITextTranslationsService<T> {
        IEnumerator<T> GetTranslations(IUnit text);
    }
}
