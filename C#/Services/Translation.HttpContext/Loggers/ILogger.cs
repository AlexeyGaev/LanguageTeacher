namespace Translation.Http.Logger {
    public interface ILogger<T> {
        void Log(T text);
    }
}