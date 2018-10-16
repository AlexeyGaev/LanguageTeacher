using Utils.Collection;

namespace Vocabulary {
    public interface IWord : IUnit {
        IUnit Transcription { get; }
        ICollection<IUnit> Translate { get; }
        ICollection<IUnit> Details { get; }
    }
}
