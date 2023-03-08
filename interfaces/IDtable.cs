using models;

namespace interfaces;

public interface IDtable
{
    IDrow GetRow(int rIndex);
    IEnumerable<IDrow> Head();
    IEnumerable<IDrow> Head(int count);
    IEnumerable<IDrow> Tail();
    IEnumerable<IDrow> Tail(int count);

    void AddRow(IDrow drow);
    
    IDcolumn GetColumn(int cIndex);
    TableInfo GetInfo();
}