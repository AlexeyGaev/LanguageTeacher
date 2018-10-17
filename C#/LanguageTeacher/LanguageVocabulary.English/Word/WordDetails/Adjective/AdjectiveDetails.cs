namespace Vocabulary.English {
    public class AdjectiveDetails : IWordDetails {
        public WordType WordType { get { return WordType.Adjective; } }
        public AdjectiveValueType ValueType { get; set; }
        public AdjectiveFormationType FormationType { get; set; }
        public AdjectiveComparisonType ComparisonType { get; set; }
    }
}
