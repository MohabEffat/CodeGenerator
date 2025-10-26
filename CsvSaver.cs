namespace CodeGenerator
{
    public class CsvSaver
    {
        public static void SaveCodesToCsv(List<string> codes, string filePath)
        {
            var lines = new List<string>();
            lines.AddRange(codes);

            File.WriteAllLines(filePath, lines);
        }
    }
}
