using Utils.Collection;

namespace LanguageVocabulary.Core {
    public interface ITextSection {
        IUnit Id { get; }
        ICollection<IUnit> Translate { get; }
    }
}
