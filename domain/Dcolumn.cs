using interfaces;
using models;

namespace domain;

public sealed class Dcolumn : IDcolumn
{
    private readonly List<object> entries = new();
    public required object Header { get; init; }
    public ColumnType Type { get; private set; }
    public object this[int index] => entries[index];
    public int Size => entries.Count;
    public void Add(object entry)
    {
        entries.Add(entry);
        
        if (Type is ColumnType.Object)
            return;
        if (!double.TryParse($"{entry}", out var d))
            Type = ColumnType.Object;
    }
    public IEnumerable<object> Head() => entries;
    public IEnumerable<object> Head(int count) => entries.Take(count);

    public IEnumerable<object> Tail()
    {
        var last = entries.Count - 1;
        while (last >= 0)
        {
            yield return entries[last];
            last -=1;
        }
    }

    public IEnumerable<object> Tail(int count)
    {
        var last = entries.Count - 1;
        var counter = 0;
        while (count >= counter)
        {
            yield return entries[last];
            last -=1;
            counter += 1;
        }
    }
}