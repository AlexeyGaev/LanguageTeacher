using Utils.Scanner;

namespace LanguageVocabulary.Core {
    public interface IVocabulary {
        IUnit Description { get; }
        IVocabularyUnit<ITextSection> Sections { get; }
        IVocabularyUnit<IWord> Words { get; }
        IScanner<IWord, ITextSection> WordsScanner { get; }
    }
}
