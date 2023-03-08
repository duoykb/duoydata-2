using models;

namespace interfaces;

public interface IDcolumn
{
    object Header { get; }
    ColumnType Type { get; }
    int Size { get; }
    object this[int index] { get; }
    void Add(object entry);
    
    IEnumerable<object> Head();
    IEnumerable<object> Head(int count);
    IEnumerable<object> Tail();
    IEnumerable<object> Tail(int count);
}