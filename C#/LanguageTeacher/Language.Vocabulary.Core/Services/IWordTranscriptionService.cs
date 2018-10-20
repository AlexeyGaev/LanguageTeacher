namespace Language.Vocabulary.Core {
    public interface IWordTranscriptionService<T> {
        T GetTranscription(IUnit word);
    }
}
