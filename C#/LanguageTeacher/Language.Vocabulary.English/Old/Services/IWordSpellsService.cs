using Utils.Collections;

namespace Language.Vocabulary.Core {
    public interface IWordSpellsService<T> {
        IEnumerator<T> GetSpells(IContextUnit word);
    }
}
