using Utils.Collection;

namespace LanguageVocabulary.Core {
    public interface IWord {
        IUnit Id { get; }
        IUnit Transcription { get; }
        ICollection<IUnit> Translate { get; }
        ICollection<IUnit> Details { get; }
    }
}
