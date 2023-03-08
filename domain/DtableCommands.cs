using System.Text.RegularExpressions;
using interfaces;

namespace domain;

public class DtableCommands
{
    public required IDtable Dtable { get; init; }

    private Dictionary<string, (Regex, Action<string>)> Patterns => new()
    {
        [nameof(TableHead)] = (new Regex(@"^\s*table\s+head\s*$", RegexOptions.IgnoreCase), TableHead),
        [nameof(TableHeadWithCount)] = (
            new Regex(@"^\s*table\s+head\s+maxrows\s+(?<n>[0-9]+)\s*$", RegexOptions.IgnoreCase), TableHeadWithCount),
        [nameof(TableTail)] = (new Regex(@"^\s*table\s+tail\s*$", RegexOptions.IgnoreCase), TableTail),
        [nameof(TableTailWithCount)] = (
            new Regex(@"^\s*table\s+tail\s+maxrows\s+(?<n>[0-9]+)\s*$", RegexOptions.IgnoreCase), TableTailWithCount),
        [nameof(TableInfo)] = (new Regex(@"^\s*table\s+info\s*$", RegexOptions.IgnoreCase), TableInfo),
        [nameof(TableRowHead)] = (new Regex(@"^\s*table\s+rowat\s+(?<n>[0-9]+)\s+head\s*$", RegexOptions.IgnoreCase),
            TableRowHead),
        [nameof(TableRowHeadWithCount)] = (
            new Regex(@"^\s*table\s+rowat\s+(?<n>[0-9]+)\s+head\s+maxentries\s+(?<c>[0-9])\s*$",
                RegexOptions.IgnoreCase), TableRowHeadWithCount),
        [nameof(TableRowTail)] = (new Regex(@"^\s*table\s+rowat\s+(?<n>[0-9]+)\s+tail\s*$", RegexOptions.IgnoreCase),
            TableRowTail),
        [nameof(TableRowTailWithCount)] = (
            new Regex(@"^\s*table\s+rowat\s+(?<n>[0-9]+)\s+tail\s+maxentries\s+(?<c>[0-9])\s*$",
                RegexOptions.IgnoreCase), TableRowTailWithCount),
        [nameof(TableColumnHead)] = (
            new Regex(@"^\s*table\s+columnat\s+(?<n>[0-9]+)\s+head\s*$", RegexOptions.IgnoreCase), TableColumnHead),
        [nameof(TableColumnHeadWithCount)] = (
            new Regex(@"^\s*table\s+columnat\s+(?<n>[0-9]+)\s+head\s+maxentries\s+(?<c>[0-9])\s*$",
                RegexOptions.IgnoreCase), TableColumnHeadWithCount),
        [nameof(TableColumnTail)] = (
            new Regex(@"^\s*table\s+columnat\s+(?<n>[0-9]+)\s+tail\s*$", RegexOptions.IgnoreCase), TableColumnTail),
        [nameof(TableColumnTailWithCount)] = (
            new Regex(@"^\s*table\s+columnat\s+(?<n>[0-9]+)\s+tail\s+maxentries\s+(?<c>[0-9])\s*$",
                RegexOptions.IgnoreCase), TableColumnTailWithCount),
    };

    private void TableHead(string cmd)
    {
        var specTable = new Table();
        specTable.Border(TableBorder.Rounded);
        var rowNumber = 0;
        foreach (var o in Dtable.GetRow(rowNumber).Head()) specTable.AddColumn($"{o}");

        var info = Dtable.GetInfo();

        while (rowNumber < info.NumberOfRows)
        {
            specTable.AddRow(Dtable.GetRow(rowNumber).Head().Select(o => o.ToString()?? "").ToArray());
            rowNumber += 1;
        }
        
        AnsiConsole.Write(specTable);
    }

    private void TableHeadWithCount(string cmd)
    {
        var n = int.Parse(Patterns[nameof(TableHeadWithCount)].Item1.Match(cmd).Groups["n"].Value);
        var specTable = new Table();
        specTable.Border(TableBorder.Rounded);
        var rowNumber = 0;
        foreach (var o in Dtable.GetRow(rowNumber).Head()) specTable.AddColumn($"{o}");

        var info = Dtable.GetInfo();

        while (rowNumber < info.NumberOfRows && rowNumber < n)
        {
            specTable.AddRow(Dtable.GetRow(rowNumber).Head().Select(o => o.ToString()?? "").ToArray());
            rowNumber += 1;
        }
        
        AnsiConsole.Write(specTable);
    }

    private void TableTail(string cmd)
    {
        var specTable = new Table();
        specTable.Border(TableBorder.Rounded);
        var info = Dtable.GetInfo();
        
        var lastRow = info.NumberOfRows - 1;
        // Why 0?? the first row has always the max number of columns??
        foreach (var o in Dtable.GetRow(0).Head()) specTable.AddColumn($"{o}");
        
        // B\se the first row is the Column header
        while (lastRow >= 1)
        {
            specTable.AddRow(Dtable.GetRow(lastRow).Head().Select(o => o.ToString()?? "").ToArray());
            lastRow -= 1;
        }
        
        AnsiConsole.Write(specTable);
    }

    private void TableTailWithCount(string cmd)
    {
        var n = int.Parse(Patterns[nameof(TableTailWithCount)].Item1.Match(cmd).Groups["n"].Value);
        var specTable = new Table();
        specTable.Border(TableBorder.Rounded);
        var info = Dtable.GetInfo();
        
        var lastRow = info.NumberOfRows - 1;
        var count = 0;
        // Why 0?? the first row has always the max number of columns??
        foreach (var o in Dtable.GetRow(0).Head()) specTable.AddColumn($"{o}");
        
        while (lastRow >= 1 && count < n)
        {
            specTable.AddRow(Dtable.GetRow(lastRow).Head().Select(o => o.ToString()?? "").ToArray());
            lastRow -= 1;
            count += 1;
        }
        
        AnsiConsole.Write(specTable);
    }

    //TODO implement TableInfo
    private void TableInfo(string cmd)
    {
    }

    private void TableRowHead(string cmd)
    {
        var n = int.Parse(Patterns[nameof(TableRowHead)].Item1.Match(cmd).Groups["n"].Value);
        
        var specTable = new Table();
        specTable.Border(TableBorder.Rounded);
        var info = Dtable.GetInfo();
        
        // Why 0?? the first row has always the max number of columns??
        foreach (var o in Dtable.GetRow(0).Head()) specTable.AddColumn($"{o}");

        specTable.AddRow(Dtable.GetRow(n).Head().Select(e => $"{e}").ToArray());
        AnsiConsole.Write(specTable);
    }

    private void TableRowHeadWithCount(string cmd)
    {
        var n = int.Parse(Patterns[nameof(TableRowHeadWithCount)].Item1.Match(cmd).Groups["n"].Value);
        var ec = int.Parse(Patterns[nameof(TableRowHeadWithCount)].Item1.Match(cmd).Groups["c"].Value);
        
        var specTable = new Table();
        specTable.Border(TableBorder.Rounded);
        var info = Dtable.GetInfo();
        
        // Why 0?? the first row has always the max number of columns??
        foreach (var o in Dtable.GetRow(0).Head().Take(ec)) specTable.AddColumn($"{o}");

        specTable.AddRow(Dtable.GetRow(n).Head(ec).Select(e => $"{e}").ToArray());
        AnsiConsole.Write(specTable);
    }

    private void TableRowTail(string cmd)
    {
        var n = int.Parse(Patterns[nameof(TableRowTail)].Item1.Match(cmd).Groups["n"].Value);

        var specTable = new Table();
        var info = Dtable.GetInfo();
        
        // Why 0?? the first row has always the max number of columns??
        foreach (var o in Dtable.GetRow(0).Tail()) specTable.AddColumn($"{o}");

        specTable.AddRow(Dtable.GetRow(n).Tail().Select(e => $"{e}").ToArray());
        AnsiConsole.Write(specTable);
    }

    private void TableRowTailWithCount(string cmd)
    {
        var n = int.Parse(Patterns[nameof(TableRowTailWithCount)].Item1.Match(cmd).Groups["n"].Value);
        var ec = int.Parse(Patterns[nameof(TableRowTailWithCount)].Item1.Match(cmd).Groups["c"].Value);
        
        var specTable = new Table();
        var info = Dtable.GetInfo();
        
        // Why 0?? the first row has always the max number of columns??
        foreach (var o in Dtable.GetRow(0).Tail().Take(ec)) specTable.AddColumn($"{o}");

        specTable.AddRow(Dtable.GetRow(n).Tail().Take(ec).Select(e => $"{e}").ToArray());
        AnsiConsole.Write(specTable);
    }

    private void TableColumnHead(string cmd)
    {
        var n = int.Parse(Patterns[nameof(TableColumnHead)].Item1.Match(cmd).Groups["n"].Value);
        var specTable = new Table();
        specTable.AddColumn($"{Dtable.GetColumn(n)[0]}");

        foreach (var e in Dtable.GetColumn(n).Head().Select(e=>$"{e}").Skip(1)) specTable.AddRow(e);
        
        AnsiConsole.Write(specTable);
    }

    private void TableColumnHeadWithCount(string cmd)
    {
        var n = int.Parse(Patterns[nameof(TableColumnHeadWithCount)].Item1.Match(cmd).Groups["n"].Value);
        var ec = int.Parse(Patterns[nameof(TableColumnHeadWithCount)].Item1.Match(cmd).Groups["c"].Value);
        
        var specTable = new Table();
        specTable.AddColumn($"{Dtable.GetColumn(n)[0]}");

        foreach (var e in Dtable.GetColumn(n).Head().Select(e=>$"{e}").Skip(1).Take(ec)) specTable.AddRow(e);
        
        AnsiConsole.Write(specTable);
        
    }

    private void TableColumnTail(string cmd)
    {
        var n = int.Parse(Patterns[nameof(TableColumnTail)].Item1.Match(cmd).Groups["n"].Value);
        
        var specTable = new Table();
        specTable.AddColumn($"{Dtable.GetColumn(n)[0]}");

        foreach (var e in Dtable.GetColumn(n)
                     .Tail()
                     .Select(e=>$"{e}")
                     .Take(Dtable.GetColumn(n).Size - 1)) specTable.AddRow(e);
        
        AnsiConsole.Write(specTable);
    }

    private void TableColumnTailWithCount(string cmd)
    {
        var n = int.Parse(Patterns[nameof(TableColumnTailWithCount)].Item1.Match(cmd).Groups["n"].Value);
        var ec = int.Parse(Patterns[nameof(TableColumnTailWithCount)].Item1.Match(cmd).Groups["c"].Value);
        
        
        var specTable = new Table();
        specTable.AddColumn($"{Dtable.GetColumn(n)[0]}");

        foreach (var e in Dtable.GetColumn(n)
                     .Tail()
                     .Select(e=>$"{e}")
                     .Take(Dtable.GetColumn(n).Size - 1)
                     .Take(ec)) specTable.AddRow(e);
        
        AnsiConsole.Write(specTable);
    }

    public void Execute(string cmd)
    {
        foreach (var (key, (pattern, action)) in Patterns)
        {
            if (!pattern.IsMatch(cmd)) continue;

            action(cmd);
            break;
        }
    }
}