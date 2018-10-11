namespace Vocabulary.English {
    public class NounDetails : IWordDetails {
        public WordType WordType { get { return WordType.Noun; } }
        public NounType NounType { get; set; }
        public NounCountableType CountableType { get; set; }

        // gender
        // number 
        // case
    }
}
