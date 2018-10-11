namespace Vocabulary.English {
    public class ArticleDetails : IWordDetails {
        public WordType WordType { get { return WordType.Article; } }
        public ArticleType Type { get; set; }
    }
}
