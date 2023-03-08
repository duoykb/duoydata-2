using interfaces;
using models;

namespace domain;

public class Dtable : IDtable
{
    private readonly List<IDcolumn> dcolumns = new();
    private int numberOfRows;
    private int maxNumberOfColumns;

    public IDcolumn GetColumn(int cIndex) => dcolumns[cIndex];

    public IDrow GetRow(int rIndex)
    {
        var row = new Drow { RowNumber = rIndex };
        foreach (var dcolumn in dcolumns)
        {
            try
            {
                row.Add(dcolumn[rIndex]);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        return row;
    }

    public IEnumerable<IDrow> Head()
    {
        var counter = 0;

        while (counter < numberOfRows)
        {
            var row = new Drow { RowNumber = counter };
            foreach (var dcolumn in dcolumns)
            {
                try
                {
                    row.Add(dcolumn[counter]);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }

            yield return row;
            counter += 1;
        }
    }

    public IEnumerable<IDrow> Head(int count)
    {
        var counter = 0;

        while (counter < numberOfRows && counter < count)
        {
            var row = new Drow { RowNumber = counter };
            foreach (var dcolumn in dcolumns)
            {
                try
                {
                    row.Add(dcolumn[counter]);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }

            yield return row;
            counter += 1;
        }
    }

    public IEnumerable<IDrow> Tail()
    {
        var last = numberOfRows -1;

        while (last >= 0)
        {
            var row = new Drow { RowNumber = last };
            foreach (var dcolumn in dcolumns)
            {
                try
                {
                    row.Add(dcolumn[last]);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }

            yield return row;
            last -= 1;
        }
    }

    public IEnumerable<IDrow> Tail(int count)
    {
        var last = numberOfRows -1;
        var counter = 0;
        
        while (last >= 0 && counter < count)
        {
            var row = new Drow { RowNumber = last };
            foreach (var dcolumn in dcolumns)
            {
                try
                {
                    row.Add(dcolumn[last]);
                }
                catch (ArgumentOutOfRangeException)
                {
                }
            }

            yield return row;
            last -= 1;
            counter += 1;
        }
    }

    public void AddRow(IDrow drow)
    {
        maxNumberOfColumns = maxNumberOfColumns < drow.Size ? drow.Size : maxNumberOfColumns;
        numberOfRows += 1;
        while (dcolumns.Count < drow.Size)
        {
            dcolumns.Add(new Dcolumn { Header = dcolumns.Count });
        }

        var index = 0;
        foreach (var entry in drow.Head())
        {
            dcolumns[index].Add(entry);
            index += 1;
        }
    }

    public TableInfo GetInfo() =>
        new()
        {
            NumberOfRows = numberOfRows,
            NumberOfColumns = maxNumberOfColumns,
            ColumnInfos = dcolumns.Select(dc => (dc.Size, dc.Type, dc.Header))
        };
}