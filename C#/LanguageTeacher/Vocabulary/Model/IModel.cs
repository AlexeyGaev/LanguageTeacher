namespace Vocabulary.Model {
    public interface IModel {
        string Name { get; }
        string Description { get; }
        IVocabularySection[] Sections { get; }
    }
}
