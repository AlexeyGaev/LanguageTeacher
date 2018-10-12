namespace Vocabulary {
    public interface IVocabularyUnit : IUnit {
        ISectionUnit[] Sections { get; }
    }
}
