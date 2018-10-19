using Utils.Collections;

namespace Language.Vocabulary.Core {
    public interface ITextWordsService {
        IEnumerator<IUnit> GetWords(string text);
    }
}
