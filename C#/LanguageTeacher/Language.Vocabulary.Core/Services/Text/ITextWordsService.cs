using Utils.Collection;

namespace Language.Vocabulary.Core {
    public interface ITextWordsService {
        ICollection<IWord> GetWords(IText text);
    }
}
