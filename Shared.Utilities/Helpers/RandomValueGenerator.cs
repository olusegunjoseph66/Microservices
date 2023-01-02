using System.Text;

namespace Shared.Utilities.Helpers
{
    public static class RandomValueGenerator
    {
        private static readonly Random random = new();
        private const string nums = "0123456789";
        private const string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ@#$&%";
        private const string LOWER_CASE = "abcdefghijklmnopqursuvwxyz";
        private const string UPPER_CAES = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NUMBERS = "123456789";
        private const string SPECIALS = @"!@£$%^&*()#€";
        
        public static string RandomString(int length, bool isNum = false)
        {
            string chars = isNum ? nums : alpha;
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateRandomDigits(int size)
        {
            var builder = new StringBuilder();
            var random = new Random();
            int character;
            for (int i = 0; i < size; i++)
            {
                character = random.Next(9);
                builder.Append(character);
            }
            return builder.ToString();
        }

        public static string GenerateRandomCode(int size = 6)
        {

            var builder = new StringBuilder();
            var randomNumber = new Random((int)DateTime.Now.Ticks);

            for (var i = 0; i < size; i++)
            {
                var character = Convert.ToInt32(Math.Floor(26 * randomNumber.NextDouble() + 18));
                builder.Append(character);
            }
            return builder.ToString();
        }

        public static string GeneratePassword(bool useLowercase, bool useUppercase, bool useNumbers, bool useSpecial,
             int passwordSize = 15)
        {
            char[] _password = new char[passwordSize];
            string charSet = ""; // Initialise to blank
            System.Random _random = new();
            int counter;

            // Build up the character set to choose from
            if (useLowercase) charSet += LOWER_CASE;

            if (useUppercase) charSet += UPPER_CAES;

            if (useNumbers) charSet += NUMBERS;

            if (useSpecial) charSet += SPECIALS;

            for (counter = 0; counter < passwordSize; counter++)
            {
                _password[counter] = charSet[_random.Next(charSet.Length - 1)];
            }

            return String.Join(null, _password);
        }
    }
}
