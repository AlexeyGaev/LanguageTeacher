namespace Vocabulary.English {
    public class Vocabulary : IVocabulary {
        public string Name { get; set; }
        public string Comment { get; set; }
        public ITextSectionCollection TextSections { get; set; }
        public IDictionary Dictionary { get; set; }
    }
}
