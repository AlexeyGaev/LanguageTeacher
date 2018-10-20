namespace Language.Vocabulary.Core {
    public interface IWordDescriptionService<T> {
        T GetDescription(IUnit unit);
    }
}
