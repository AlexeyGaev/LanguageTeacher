using Language.Common.Utils;

namespace Language.Vocabulary.Services {
    public interface ITextWordsService {
        IEnumerator<IUnit> GetWords(string text);
    }
}
