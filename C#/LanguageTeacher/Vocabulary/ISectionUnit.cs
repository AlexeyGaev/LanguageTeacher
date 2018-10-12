namespace Vocabulary {
    public interface ISectionUnit : IUnit {
        IUnit Transcription { get; }
        IUnit[] Translate { get; }
        IUnit[] Details { get; }
    }
}
