using domain;
using interfaces;

namespace tests;

public class DrowShould
{
    private readonly IDrow drow = new Drow { RowNumber = 1 };
    
    [Fact]
    public void AddEntries()
    {
        drow.Add("dd");
        drow.Add("dd");
        
        Assert.True(drow.Size is 2);
    }

    [Fact]
    public void ImplementHead()
    {
     drow.Add(1);
     drow.Add(2);
     var vals = new[] { 1, 2 };
     foreach (var (f,s) in drow.Head().Zip(vals)) Assert.Equal(s, (int)f);
    }

    [Fact]
    public void ImplementHeadWithCount()
    {
        drow.Add(1);
        drow.Add(2);
        drow.Add(3);
        var vals = new[] { 1, 2 };
        foreach (var (f,s) in drow.Head(2).Zip(vals)) Assert.Equal(s, (int)f);
    }

    [Fact]
    public void ImplementTail()
    {
        drow.Add(2);
        drow.Add(3);
        var vals = new[] { 3,2 };
        foreach (var (f,s) in drow.Tail().Zip(vals)) Assert.Equal(s, (int)f);
    }

    [Fact]
    public void ImplementTailWithCount()
    {
        drow.Add(1);
        drow.Add(2);
        drow.Add(3);
        var vals = new[] { 3, 2 };
        foreach (var (f,s) in drow.Tail(2).Zip(vals)) Assert.Equal(s, (int)f);
    }
}