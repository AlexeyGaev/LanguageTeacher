using Utils.Collections;

namespace Language.Vocabulary.Core {
    public interface IWordSpellsService<T> {
        ICollection<T> GetSpells(IContextUnit word);
    }
}
