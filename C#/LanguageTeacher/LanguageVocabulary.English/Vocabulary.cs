using LanguageVocabulary.Core;

namespace LanguageVocabulary.English {
    public class Vocabulary : IVocabulary {
        public IUnit Description { get; set; }
        public IVocabularyItems<ITextSection> Sections { get; set; }
        public IVocabularyItems<IWord> Words { get; set; }
    }
}
