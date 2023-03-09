using domain;
using Spectre.Console;

var dtable = DtableBuilder.FromCsv(@"C:\Users\kb\Desktop\WD\myprojects\duoydata\app\csvFiles\sample-1.csv");
var cmd = new DtableCommands{Dtable = dtable};

string? input;
var txt = new FigletText("duoy Data");
AnsiConsole.Write(txt);
while ((input = AnsiConsole.Ask<string>(">>> ")).Trim().ToLower() is not null or "exit") cmd.Execute(input);