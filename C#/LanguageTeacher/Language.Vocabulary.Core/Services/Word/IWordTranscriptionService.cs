using Utils.Collection;

namespace Language.Vocabulary.Core {
    public interface IWordTranscriptionService<T> {
        T GetTranscription(IWord word);
    }
}
