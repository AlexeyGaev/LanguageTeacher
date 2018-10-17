namespace Vocabulary.English {
    public class Suggestion {
        public string Name { get; set; }
        public Translate[] Translates { get; set; }
        public SuggestionType Type { get; set; }
        public string Comment { get; set; }
    }
}
