using domain;
using Spectre.Console;

var dtable = DtableBuilder.FromCsv(@"C:\Users\kb\Desktop\WD\myprojects\duoydata\app\csvFiles\sample-1.csv");
var cmd = new DtableCommands{Dtable = dtable};

AnsiConsole.Write(new FigletText("duoy Data"));
while (true)
{
    var input = AnsiConsole.Ask<string>(">>> ").Trim().ToLower();
    if (input is "exit")
        return;
    cmd.Execute(input);
}