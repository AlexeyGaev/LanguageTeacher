using Utils.Collections;

namespace Language.Vocabulary.Core {
    public interface ITextWordsService {
        IEnumerator<IContextUnit> GetWords(IUnit text);
    }
}
