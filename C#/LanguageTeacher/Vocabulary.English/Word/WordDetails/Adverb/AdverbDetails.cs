namespace Vocabulary.English {
    public class AdverbDetails : IWordDetails {
        public WordType WordType { get { return WordType.Adverb; } }
        public AdverbType AdverbType { get; set; }
    }
}
