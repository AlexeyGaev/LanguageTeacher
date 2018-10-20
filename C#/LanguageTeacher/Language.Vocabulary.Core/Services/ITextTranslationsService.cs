using Utils.Collections;

namespace Language.Vocabulary.Core {
    public interface ITextTranslationsService {
        IEnumerator<IUnit> GetTranslations(string text);
    }
}
