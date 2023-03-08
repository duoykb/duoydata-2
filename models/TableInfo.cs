namespace models;

public class TableInfo
{
    public required int NumberOfRows { get; set; }
    public required int NumberOfColumns { get; set; }
    public required IEnumerable<(int columnSize, ColumnType columnType, object columnHeader)> ColumnInfos { get; set; }
}