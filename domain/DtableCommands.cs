using System.Text.RegularExpressions;
using interfaces;

namespace domain;

public class DtableCommands
{
    public required IDtable Dtable { get; init; }

    private Dictionary<string, (Regex, Action<string>)> Patterns => new()
    {
        [nameof(IndexRow)] = (
            new Regex(@"^r\[\s*(?<start>e?[0-9]+)\s*:\s*(?<range>e?[0-9]+)\s*\]$"), IndexRow),
        [nameof(IndexColumn)] = (
            new Regex(@"^c\[\s*(?<start>e?[0-9]+)\s*:\s*(?<range>e?[0-9]+)\s*\]$"), IndexColumn),
        [nameof(IndexRowAndColumn)] =
            (new Regex(@"^(
                           r\[\s*(?<startR>e?[0-9]+)\s*:\s*(?<rangeR>e?[0-9]+)\s*\]\s+c\[\s*(?<startC>e?[0-9]+)\s*:\s*(?<rangeC>e?[0-9]+)\s*\]
                            |
                           c\[\s*(?<startC>e?[0-9]+)\s*:\s*(?<rangeC>e?[0-9]+)\s*\]\s+r\[\s*(?<startR>e?[0-9]+)\s*:\s*(?<rangeR>e?[0-9]+)\s*\]
                          )$", RegexOptions.IgnorePatternWhitespace), IndexRowAndColumn)
    };

    private void IndexRow(string cmd)
    {
        var match = Patterns[nameof(IndexRow)].Item1.Match(cmd);
        var start = match.Groups["start"].Value;
        var range = match.Groups["range"].Value;

        var lastRowIndex = Dtable.GetInfo().NumberOfRows - 1;
        var sIndex = start.StartsWith("e") ? lastRowIndex - int.Parse(start.Replace("e", "")) : int.Parse(start);
        var eIndex = range.StartsWith("e") ? lastRowIndex - int.Parse(range.Replace("e", "")) : int.Parse(range);
        var increment = sIndex < eIndex ? 1 : -1;
        var condition = bool(int sI, int eI) => increment < 0 ? sI > eI : eI > sI;

        var specTable = new Table();
        // [0:0] unless there is a check it will add the first row as columns 
        if (condition(sIndex, eIndex))
        {
            foreach (var entry in Dtable.GetRow(sIndex).Head()) specTable.AddColumn($"{entry}");
            sIndex += increment;
        }

        while (condition(sIndex, eIndex))
        {
            specTable.AddRow(Dtable.GetRow(sIndex).Head().Select(entry => $"{entry}").ToArray());
            sIndex += increment;
        }

        AnsiConsole.Write(specTable);
    }

    private void IndexColumn(string cmd)
    {
        var match = Patterns[nameof(IndexColumn)].Item1.Match(cmd);
        var start = match.Groups["start"].Value;
        var range = match.Groups["range"].Value;

        var lastColumnIndex = Dtable.GetInfo().NumberOfColumns - 1;
        var sIndex = start.StartsWith("e") ? lastColumnIndex - int.Parse(start.Replace("e", "")) : int.Parse(start);
        var eIndex = range.StartsWith("e") ? lastColumnIndex - int.Parse(range.Replace("e", "")) : int.Parse(range);
        var increment = sIndex < eIndex ? 1 : -1;
        var condition = bool(int sI, int eI) => increment < 0 ? sI > eI : eI > sI;

        var specTable = new Table();
        var maxNumberOfRows = Dtable.GetInfo().NumberOfRows;
        var entryIndex = 0;

        var tempStartIndex = sIndex;
        var tempEndIndex = eIndex;

        // example: c[0:0]   no column is selected in this case return
        if (condition(sIndex, eIndex) is false)
            return;

        while (condition(tempStartIndex, tempEndIndex))
        {
            var column = Dtable.GetColumn(tempStartIndex);

            tempStartIndex += increment;

            // why continue? for inconsistent table
            if (column.Size <= entryIndex)
                continue;
            specTable.AddColumn($"{column[entryIndex]}");
        }

        entryIndex += 1;
        while (maxNumberOfRows > entryIndex)
        {
            tempStartIndex = sIndex;
            tempEndIndex = eIndex;
            var entries = new LinkedList<string>();
            while (condition(tempStartIndex, tempEndIndex))
            {
                var column = Dtable.GetColumn(tempStartIndex);
                tempStartIndex += increment;
                if (column.Size <= entryIndex)
                    continue;
                entries.AddLast($"{column[entryIndex]}");
            }

            specTable.AddRow(entries.ToArray());
            entryIndex += 1;
        }

        AnsiConsole.Write(specTable);
    }

    private void IndexRowAndColumn(string cmd)
    {
        var match = Patterns[nameof(IndexRowAndColumn)].Item1.Match(cmd);

        var startR = match.Groups["startR"].Value;
        var rangeR = match.Groups["rangeR"].Value;
        var startC = match.Groups["startC"].Value;
        var rangeC = match.Groups["rangeC"].Value;

        var info = Dtable.GetInfo();
        var lastRowIndex = info.NumberOfRows - 1;
        var lastColumnIndex = info.NumberOfColumns - 1;

        var startRowIndex = startR.StartsWith("e")
            ? lastRowIndex - int.Parse(startR.Replace("e", ""))
            : int.Parse(startR);
        var rangeRowIndex = rangeR.StartsWith("e")
            ? lastRowIndex - int.Parse(rangeR.Replace("e", ""))
            : int.Parse(rangeR);
        var incrementR = startRowIndex > rangeRowIndex ? -1 : 1;


        var startColumnIndex = startC.StartsWith("e")
            ? lastColumnIndex - int.Parse(startC.Replace("e", ""))
            : int.Parse(startC);
        var rangeColumnIndex = rangeC.StartsWith("e")
            ? lastColumnIndex - int.Parse(rangeC.Replace("e", ""))
            : int.Parse(rangeC);
        var incrementC = startColumnIndex > rangeColumnIndex ? -1 : 1;

        var condition = bool(int start, int range, int increment) => increment switch
        {
            -1 => start > range,
            1 => range > start,
            _ => throw new ArgumentException("increment must be 1 or -1")
        };
        if (!(condition(startRowIndex, rangeRowIndex, incrementR) &&
              condition(startColumnIndex, rangeColumnIndex, incrementC)))
            return;

        var specTable = new Table();


        // foreach (var i in Enumerable.Range(0, Math.Abs(startColumnIndex - rangeColumnIndex)))
        //     specTable.AddColumn($"");

        var tempStartColumnIndex = startColumnIndex;

        if (condition(startRowIndex, rangeRowIndex, incrementR))
        {
            while (condition(tempStartColumnIndex, rangeColumnIndex, incrementC))
            {
                var column = Dtable.GetColumn(tempStartColumnIndex);
                tempStartColumnIndex += incrementC;
                specTable.AddColumn($"{column[0]}");
            }

            if (startRowIndex is 0)
                startRowIndex += incrementR;
            tempStartColumnIndex = startColumnIndex;
        }

        while (condition(startRowIndex, rangeRowIndex, incrementR))
        {
            var entries = new LinkedList<string>();

            while (condition(tempStartColumnIndex, rangeColumnIndex, incrementC))
            {
                var column = Dtable.GetColumn(tempStartColumnIndex);
                tempStartColumnIndex += incrementC;

                if (startRowIndex >= column.Size)
                    continue;
                entries.AddLast($"{column[startRowIndex]}");
            }

            specTable.AddRow(entries.ToArray());
            startRowIndex += incrementR;
            tempStartColumnIndex = startColumnIndex;
        }

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