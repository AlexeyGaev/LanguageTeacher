namespace Vocabulary.English {
    public class Word {
        public string Name { get; set; }
        public string Comment { get; set; }
        public Translate[] Translates { get; set; }
        public IWordDetails Details { get; set; }
    }
}
