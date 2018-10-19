namespace Language.Vocabulary.Core {
    public interface IDescriptionService<T> {
        T GetDescription(IContextUnit unit);
    }
}
