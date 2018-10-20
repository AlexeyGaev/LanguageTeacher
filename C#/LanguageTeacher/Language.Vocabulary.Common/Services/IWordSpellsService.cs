using Language.Common.Utils;

namespace Language.Vocabulary.Services {
    public interface IWordSpellsService<T> {
        IEnumerator<T> GetSpells(IUnit word);
    }
}
