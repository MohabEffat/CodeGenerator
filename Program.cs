using CodeGenerator;
using System.Diagnostics;

var codes = new List<string>();

var codeGenerator = new uCodeGenerator();

Stopwatch stopwatch = new Stopwatch();

stopwatch.Start();

for (int i = 0; i < 1_000_000; i++)
{
    var code = codeGenerator.GenerateCode("TJ");

    codes.Add(code);
}

CsvSaver.SaveCodesToCsv(codes, ""); // <-- Specify your desired file path here

stopwatch.Stop();

Console.WriteLine("-------------------------");

Console.WriteLine($"Time taken to generate 1,000,000 codes: {stopwatch.ElapsedMilliseconds} ms");
