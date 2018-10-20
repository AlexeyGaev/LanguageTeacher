using Language.Common.Utils;

namespace Language.Vocabulary.Services {
    public interface ITextTranslationsService {
        IEnumerator<IUnit> GetTranslations(string text);
    }
}
