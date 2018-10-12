namespace Vocabulary.Model {
    public interface IVocabularySection : IUnit {
        ISectionUnit[] Sections { get; }
    }
}
