using CodeGenerator;

Console.WriteLine("Generated Code: TJ25105033MD");

var generator = new uCodeCalculator("TJ25105033MD");
var weights = generator.CalculateWeights();
Console.WriteLine($"Weights: {weights}");

var checksum = generator.CalculateChecksum();
Console.WriteLine($"Checksum: {checksum}");

var fullCode = generator.GenerateFullCode();
Console.WriteLine($"Full Code: {fullCode}");

Console.WriteLine("-------------------------");

var codeGenerator = new uCodeGenerator();
var newCode = codeGenerator.GenerateCode("TJ");
Console.WriteLine($"Generated Code: {newCode}");

var uCodeCalculator = new uCodeCalculator(newCode);
var newWeights = uCodeCalculator.CalculateWeights();
Console.WriteLine($"Weights: {newWeights}");

var newChecksum = uCodeCalculator.CalculateChecksum();
Console.WriteLine($"Checksum: {newChecksum}");

var newFullCode = uCodeCalculator.GenerateFullCode();
Console.WriteLine($"Full Code: {newFullCode}");

Console.WriteLine("-------------------------");

//var codes = new List<string>();

//Stopwatch stopwatch = new Stopwatch();

//stopwatch.Start();

//for (int i = 0; i < 1_000_000; i++)
//{
//    codes.Add(codeGenerator.GenerateCode("TJ"));
//}

//stopwatch.Stop();

//CsvSaver.SaveCodesToCsv(codes, "D:\\.Net Projects\\CodeGenerator\\codes.csv");

//Console.WriteLine("-------------------------");

//Console.WriteLine($"Time taken to generate 1,000,000 codes: {stopwatch.ElapsedMilliseconds} ms");
