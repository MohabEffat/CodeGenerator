namespace CodeGenerator
{
    public class Helper
    {
        public static readonly Dictionary<char, int>  Values = new Dictionary<char, int>
        {
            { '0', 2 },
            { '1', 21 },
            { '2', 30 },
            { '3', 10 },
            { '4', 4 },
            { '5', 3 },
            { '6', 16 },
            { '7', 5 },
            { '8', 27 },
            { '9', 28 },
            { 'B', 22 },
            { 'C', 9 },
            { 'D', 15 },
            { 'F', 23 },
            { 'G', 6 },
            { 'H', 24 },
            { 'J', 12 },
            { 'K', 18 },
            { 'L', 19 },
            { 'M', 7 },
            { 'N', 29 },
            { 'P', 20 },
            { 'Q', 11 },
            { 'R', 17 },
            { 'S', 25 },
            { 'T', 14 },
            { 'V', 26 },
            { 'W', 8 },
            { 'X', 13 },
            { 'Z', 1 }
        };
        public static int CalculateWeights(string code)
        {
            int sum = 0;
            for (int i = 0; i < code.Length; i++)
            {
                char c = code[i];
                if (Values.TryGetValue(c, out int value))
                {
                    sum += value;
                }
            }
            return sum;
        }
        public static int CalculateChecksum(string code)
        {
            int Weight = CalculateWeights(code);

            int firDigit = Weight % 10;
            int secDigit = (Weight / 10) % 10;
            int thirdDigit = (Weight / 100);

            int sumOfModules = 0;

            int[] arrayOfValuesAndWeights = { thirdDigit, secDigit, firDigit };

            for (int i = 0; i < code.Length; i++)
            {
                char c = code[i];
                if (Values.TryGetValue(c, out int value))
                {
                    sumOfModules += value * arrayOfValuesAndWeights[i % 3];
                }
            }

            int roundedSum = (int)(Math.Ceiling(sumOfModules / 10.0) * 10);
            int checksum = roundedSum - sumOfModules;
            return checksum;
        }

    }
    public class uCodeGenerator
    {
        public static readonly HashSet<char> AllowedPrefixChars = new HashSet<char>
        {
            'B', 'C', 'D', 'F', 'G', 'J', 'K', 'L', 'P', 'Q', 'R', 'T', 'V'
        };

        private readonly char[] charsWithoutVowels =
        {
            'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M',
            'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Z'
        };

        private readonly string _year;

        private int _sequence = 0;

        public uCodeGenerator(string? year = "25")
        {
            if (string.IsNullOrWhiteSpace(year) || year!.Length != 2 || !year.All(char.IsDigit))
                throw new ArgumentException("Year must be a 2-digit string (e.g., '25').");
            _year = year;
        }

        public string GenerateCode(string prefix)
        {
            if (prefix.Length != 2 || !prefix.All(c => AllowedPrefixChars.Contains(c)))
                throw new ArgumentException("Prefix must be exactly 2 allowed characters.");

            if (_sequence > 999999)
                throw new InvalidOperationException("All possible codes have been generated.");

            string numericPart = _sequence.ToString("D6");
            _sequence++;

            Random random = new Random();

            char[] codeChars = new char[12];

            codeChars[0] = prefix[0];

            codeChars[1] = prefix[1];

            codeChars[2] = _year[0];

            codeChars[3] = _year[1];


            for (int i = 0; i < 6; i++)
                codeChars[4 + i] = numericPart[i];

            codeChars[10] = charsWithoutVowels[random.Next(charsWithoutVowels.Length)];
            codeChars [11] = charsWithoutVowels[random.Next(charsWithoutVowels.Length)];

            var code = new string(codeChars);
            var fullCode = code + Helper.CalculateChecksum(code);
            return fullCode;
        }
    }
}
