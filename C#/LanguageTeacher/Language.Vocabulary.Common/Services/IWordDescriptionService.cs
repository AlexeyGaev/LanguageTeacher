namespace Language.Vocabulary.Services {
    public interface IWordDescriptionService<T> {
        T GetDescription(IUnit unit);
    }
}
