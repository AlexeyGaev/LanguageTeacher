namespace Vocabulary {
    public interface ICollection<T> where T : IUnit {
        int Count { get; }
        T this[int index] { get; set; }
    }
}
