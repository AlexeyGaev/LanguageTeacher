using Utils.Collection;

namespace Vocabulary {
    public interface ITextSection : IUnit {
        ICollection<IUnit> Translate { get; }
    }
}
