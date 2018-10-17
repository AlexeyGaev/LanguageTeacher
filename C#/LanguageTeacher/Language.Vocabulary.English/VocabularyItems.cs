using Utils.Collection;
using LanguageVocabulary.Core;

namespace LanguageVocabulary.English {
    public class VocabularyItems<T> : IVocabularyItems<T> {
        public IUnit Description { get; set; }
        public ICollection<T> Items { get; set; }    
    }
}
