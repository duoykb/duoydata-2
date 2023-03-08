using domain;
using interfaces;
using models;

namespace tests;

public class DtableShould
{
    public DtableShould()
    {
        foreach (var n in Enumerable.Range(1, 3)) row1.Add(n);
        foreach (var n in Enumerable.Range(1, 3)) row3.Add(n);

        row2.Add(1);
        row2.Add("a");
        row2.Add(4);
        row4.Add(1);
        row4.Add(2);
    }

    private readonly IDtable dtable = new Dtable();

    private readonly Drow row1 = new() { RowNumber = 1 }; // 1 2 3
    private readonly Drow row2 = new() { RowNumber = 2 }; // 1 a 4
    private readonly Drow row3 = new() { RowNumber = 3 }; // 1 2 3
    private readonly Drow row4 = new() { RowNumber = 4 }; // 1 2

    private static IEnumerable<Drow> GenRows(int numberOfRows, int rowSize)
    {
        foreach (var i in Enumerable.Range(0, numberOfRows))
        {
            var r = new Drow { RowNumber = 1 };
            foreach (var rowEntry in Enumerable.Range(0, rowSize)) r.Add(rowEntry);

            yield return r;
        }
    }

    [Fact]
    public void AddRow() => dtable.AddRow(row1);

    [Fact]
    public void ReturnValidTableInfo()
    {
        dtable.AddRow(row1);
        dtable.AddRow(row3);
        var info = dtable.GetInfo();
        Assert.True(info.NumberOfColumns is 3);
        Assert.True(info.NumberOfRows is 2);

        var index = 0;

        foreach (var (size, type, header) in info.ColumnInfos)
        {
            Assert.True(size is 2);
            Assert.Equal(index, header);
            Assert.True(type is ColumnType.Number);
            index += 1;
        }
    }

    [Fact]
    public void ReturnValidTableInfo2()
    {
        /*
         * 1 2 3
         * 1 a 4
         * 1 2 3
         * 1 2
         */

        dtable.AddRow(row1);
        dtable.AddRow(row2);
        dtable.AddRow(row3);
        dtable.AddRow(row4);

        var index = 0;
        var info = dtable.GetInfo();
        foreach (var (size, type, header) in info.ColumnInfos)
        {
            if (index is 0)
            {
                Assert.True(size is 4);
                Assert.True(type is ColumnType.Number);
            }
            else if (index is 1)
            {
                Assert.True(size is 4);
                Assert.True(type is ColumnType.Object);
            }
            else if (index is 2)
            {
                Assert.True(size is 3);
                Assert.True(type is ColumnType.Number);
            }

            Assert.Equal(index, (int)header);
            index += 1;
        }
    }

    [Fact]
    public void GetRow()
    {
        dtable.AddRow(row2);
        dtable.AddRow(row1);

        foreach (var (f, s) in dtable.GetRow(1).Head().Zip(row1.Head()))
            Assert.Equal(f, s);
    }

    [Fact]
    public void GetColumn()
    {
        /*
         * 1 a 4
         * 1 2
         */

        dtable.AddRow(row2);
        dtable.AddRow(row4);

        var c2 = dtable.GetColumn(2);
        var c1 = dtable.GetColumn(1);
        var c0 = dtable.GetColumn(0);

        Assert.Equal(1, c2.Size);
        Assert.Equal(ColumnType.Number, c2.Type);

        Assert.Equal(ColumnType.Object, c1.Type);
        Assert.Equal(2, c1.Size);

        Assert.Equal(ColumnType.Number, c0.Type);
        Assert.Equal(2, c0.Size);
    }

    [Fact]
    public void ImplementHead()
    {
        /*
        * 1 a 4
        * 1 2
        */

        dtable.AddRow(row2);
        dtable.AddRow(row4);

        var rows = dtable.Head().ToList();

        foreach (var i in Enumerable.Range(0, 2))
        {
            IDrow r = i switch
            {
                0 => row2,
                1 => row4,
                _=> throw new Exception("...")
            };
            foreach (var (f, s) in r.Head().Zip(rows[i].Head())) Assert.Equal(f, s);
        }
    }

    [Fact]
    public void ImplementHeadWithCount()
    {
        dtable.AddRow(row2);
        dtable.AddRow(row4);

        var rows = dtable.Head(1).ToList();
        foreach (var (f, s) in row2.Head().Zip(rows[0].Head())) Assert.Equal(f, s);
        
    }

    [Fact]
    public void ImplementTail()
    {
        dtable.AddRow(row2);
        dtable.AddRow(row4);
        
        var rows = dtable.Tail().ToList();

        foreach (var i in Enumerable.Range(0, 2))
        {
            IDrow r = i switch
            {
                0 => row4,
                1 => row2,
                _=> throw new Exception("...")
            };
            foreach (var (f, s) in r.Head().Zip(rows[i].Head())) Assert.Equal(f, s);
        }
    }

    [Fact]
    public void ImplementTailWithCount()
    {
        dtable.AddRow(row2);
        dtable.AddRow(row4);

        var r = dtable.Tail(1).Single();
        foreach (var (f, s) in r.Head().Zip(row4.Head())) Assert.Equal(f, s);
        
    }
}