namespace LanguageVocabulary.Core {
    public interface IVocabulary {
        IUnit Description { get; }
        IVocabularyItems<ITextSection> Sections { get; }
        IVocabularyItems<IWord> Words { get; }
    }
}
