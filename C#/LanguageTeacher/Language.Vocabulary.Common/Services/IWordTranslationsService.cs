using Language.Common.Utils;

namespace Language.Vocabulary.Services {
    public interface IWordTranslationsService<T> {
        IEnumerator<T> GetTranslations(IUnit word);
    }
}
