using Utils.Collection;

namespace Vocabulary.English {
    public class TextSectionCollection : Collection<ITextSection>, ITextSectionCollection {
        public string Name { get; set; }
        public string Comment { get; set; }
    }
}
