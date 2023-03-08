using System.Text;
using interfaces;

namespace domain;

public static class DtableBuilder
{
    public static IDtable FromCsv(string filePath)
    {
        var dtable = new Dtable();
        var builder = new StringBuilder();
        
        foreach (var line in File.ReadLines(filePath))
        {
            var drow = new Drow { RowNumber = 0 };
            foreach (var charr in line)
            {
                if (charr is ',')
                {
                    drow.Add($"{builder}");
                    builder.Clear();
                }
                else
                    builder.Append(charr);
            }

            var s = $"{builder}";
            if (s is not "") drow.Add(s);
            builder.Clear();
            dtable.AddRow(drow);
        }
        return dtable;
    }
}