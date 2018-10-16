using Utils.Collection;

namespace Vocabulary.English {
    public class Dictionary : IDictionary {
        public string Name { get; set; }
        public string Comment { get; set; }
        public ICollection<IWord> Words { get; set; }
    }
}
