namespace Vocabulary {
    public interface IVocabulary : IUnit {
        ITextSectionCollection TextSections { get; }
        IDictionary Dictionary { get; }
    }
}
