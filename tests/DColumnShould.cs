using domain;
using interfaces;
using models;

namespace tests;

public class DcolumnShould
{
    private readonly IDcolumn dcolumn = new Dcolumn { Header = 1 };
    
    [Fact]
    public void AddEntries()
    {
        dcolumn.Add("tht");
        dcolumn.Add(12);
        Assert.True(dcolumn.Size is 2);
    }

    [Fact]
    public void TrackTheTypeOfEntries()
    {
        dcolumn.Add(10);
        dcolumn.Add(0.1);
        Assert.True(dcolumn.Type is ColumnType.Number);
        
        dcolumn.Add("m");
        dcolumn.Add(10);
        Assert.True(dcolumn.Type is ColumnType.Object);
    }

    [Fact]
    public void ImplementHead()
    {
        dcolumn.Add(1);
        dcolumn.Add(2);
        var vals = new[] { 1, 2 };
        foreach (var (f, s) in dcolumn.Head().Zip(vals)) 
            Assert.Equal(s, (int)f);
    }

    [Fact]
    public void ImplementHeadWithCount()
    {
        dcolumn.Add(1);
        dcolumn.Add(2);
        dcolumn.Add(3);
        var vals = new[] { 1, 2 };
        foreach (var (f, s) in dcolumn.Head(2).Zip(vals)) 
            Assert.Equal(s, (int)f);
    }

    [Fact]
    public void ImplementTail()
    {
        dcolumn.Add(1);
        dcolumn.Add(2);
        var vals = new[] { 2, 1 };
        foreach (var (f,s) in dcolumn.Tail().Zip(vals)) Assert.Equal(s, (int)f);
    }

    [Fact]
    public void ImplementTailWithCount()
    {
        dcolumn.Add(1);
        dcolumn.Add(2);
        dcolumn.Add(3);
        var vals = new[] { 3, 2 };
        foreach (var (f, s) in dcolumn.Tail(2).Zip(vals)) Assert.Equal(s, (int)f);
    }
}