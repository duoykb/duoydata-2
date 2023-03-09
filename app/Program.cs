using domain;
using Spectre.Console;

var dtable = DtableBuilder.FromCsv(@"C:\Users\kb\Desktop\WD\myprojects\duoydata\app\csvFiles\sample-1.csv");
var cmd = new DtableCommands{Dtable = dtable};

string? input;
AnsiConsole.Write(new FigletText("duoy Data"));
while ((input = AnsiConsole.Ask<string>(">>> ")).Trim().ToLower() is not null or "exit") cmd.Execute(input);