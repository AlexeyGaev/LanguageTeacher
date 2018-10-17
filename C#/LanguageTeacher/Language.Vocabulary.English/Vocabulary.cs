using LanguageVocabulary.Core;
using Utils.Scanner;

namespace LanguageVocabulary.English {
    public class Vocabulary : IVocabulary {
        public IUnit Description { get; set; }
        public IVocabularyUnit<ITextSection> Sections { get; set; }
        public IVocabularyUnit<IWord> Words { get; set; }
        public IScanner<IWord, ITextSection> WordsScanner { get; set; }
    }
}
