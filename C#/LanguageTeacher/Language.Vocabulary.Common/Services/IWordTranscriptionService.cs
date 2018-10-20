namespace Language.Vocabulary.Services {
    public interface IWordTranscriptionService<T> {
        T GetTranscription(IUnit word);
    }
}
