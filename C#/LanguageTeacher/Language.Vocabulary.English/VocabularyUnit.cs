using Utils.Collection;
using LanguageVocabulary.Core;

namespace LanguageVocabulary.English {
    public class VocabularyUnit<T> : IVocabularyUnit<T> {
        public IUnit Description { get; set; }
        public ICollection<T> Items { get; set; }    
    }
}
