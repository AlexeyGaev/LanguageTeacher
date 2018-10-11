namespace Vocabulary.English {
    public class NumeralsDetails : IWordDetails {
        public WordType WordType { get { return WordType.Numerals; } }
        public NumeralsType Type { get; set; }
    }
}
