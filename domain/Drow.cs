using interfaces;

namespace domain;

public class Drow : IDrow
{
    private readonly List<object> entries = new();
    public int Size => entries.Count;
    public required int RowNumber { get; init; }
    public void Add(object value) => entries.Add(value);
    public object this[int index] => entries[index];
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

        var counter = 0;
        var last = entries.Count - 1;
        while (last >= 0 && count > counter)
        {
            yield return entries[last];
            last -=1;
            counter += 1;
        }
    }
}