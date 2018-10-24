namespace Translation.Http.Converter {
    public interface IToStringConverter<T> {
        string Convert(T value);
    }
}