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
    }

    public class uCodeCalculator
    {
        private readonly string _code;
        public uCodeCalculator(string code)
        {
            _code = code;
        }

        public int CalculateWeights()
        {
            int sum = 0;
            for (int i = 0; i < _code.Length; i++)
            {
                char c = _code[i];
                if (Helper.Values.TryGetValue(c, out int value))
                {
                    sum += value;
                }
            }
            return sum ;
        }
        public int CalculateChecksum()
        {
            int Weight = CalculateWeights();

            int firDigit = Weight % 10;
            int secDigit = (Weight / 10) % 10;
            int thirdDigit = (Weight / 100);

            int sumOfModules = 0;

            int[] arrayOfValuesAndWeights = { thirdDigit, secDigit, firDigit };

            for (int i = 0; i < _code.Length; i++)
            {
                char c = _code[i];
                if (Helper.Values.TryGetValue(c, out int value))
                {
                    sumOfModules += value * arrayOfValuesAndWeights[i % 3];
                }
            }

            int roundedSum = (int)(Math.Ceiling(sumOfModules / 10.0) * 10);
            int checksum = roundedSum - sumOfModules;
            return checksum;
        }
        public string GenerateFullCode()
        {
            int checksum = CalculateChecksum();
            return String.Concat(_code, checksum.ToString());
        }
    }


    public class uCodeGenerator
    {
        private readonly char[] chars =
        {
            'B', 'C', 'D', 'F', 'G', 'J', 'K', 'L', 'P', 'Q', 'R', 'T', 'V'
        };

        private readonly char[] charsWithoutVowels =
        {
            'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M',
            'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Z'
        };

        private readonly string _year = "25";

        public string GenerateCode(string prefix)
        {
            if (prefix.Length != 2 || !prefix.All(c => chars.Contains(c)))
                throw new ArgumentException("Prefix must be exactly 2 allowed characters.");

            Random random = new Random();

            char[] codeChars = new char[12];

            codeChars[0] = prefix[0];

            codeChars[1] = prefix[1];

            codeChars[2] = _year[0];

            codeChars[3] = _year[1];

            for (int i = 4; i < 10; i++)
            {
                codeChars[i] = (char)('0' + random.Next(10));
            }

            codeChars [10] = charsWithoutVowels[random.Next(charsWithoutVowels.Length)];
            codeChars [11] = charsWithoutVowels[random.Next(charsWithoutVowels.Length)];

            return new string(codeChars);
        }

        private string a = "0";
        private string b = "10";
        private string c = "100";

        private string Next()
        {
            int ai = int.Parse(a);
            int bi = int.Parse(b);
            int ci = int.Parse(c);

            ai++;

            if (ai > 9)
            {
                ai = 0;
                bi++;
            }

            if (bi > 99)
            {
                bi = 10;
                ci++;
            }

            if (ci > 999)
                throw new InvalidOperationException("Sequence overflow (C > 999)");

            a = ai.ToString("0");
            b = bi.ToString("00");
            c = ci.ToString("000");

            return $"{a}{b}{c}";
        }
    }
}
