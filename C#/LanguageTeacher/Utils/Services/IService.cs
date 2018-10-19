namespace Utils.Services {
    public interface IService<S, T> {
        T Func(S item);
    }
}
