namespace interfaces;

public interface IDrow
{
    int Size { get; }
    int RowNumber { get; }
    void Add(object value);
    object this[int index] { get; }
    IEnumerable<object> Head();
    IEnumerable<object> Head(int count);

    IEnumerable<object> Tail();
    IEnumerable<object> Tail(int count);
}