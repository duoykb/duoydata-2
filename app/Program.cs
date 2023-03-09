using domain;
using Spectre.Console;



AnsiConsole.Write(new FigletText("duoy Data"));

var csvFile = AnsiConsole.Ask<string>("csv file: ");
var dtable = DtableBuilder.FromCsv(csvFile);
var tableCommands = new DtableCommands{Dtable = dtable};

while (true)
{
    var input = AnsiConsole.Ask<string>(">>> ").Trim().ToLower();
    if (input is "exit")
        return;
    tableCommands.Execute(input);
}