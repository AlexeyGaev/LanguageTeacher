using Utils.Collection;

namespace Vocabulary {
    public interface IDictionary : IUnit {
        ICollection<IWord> Words { get; }
    }
}
