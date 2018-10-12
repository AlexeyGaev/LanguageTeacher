namespace Vocabulary.Model {
    public interface ISectionUnit : IUnit {
        ITranscriptionUnit Transcription { get; }
        ITranslateUnit[] Translate { get; }
        IDetailUnit[] Details { get; }
    }
}
