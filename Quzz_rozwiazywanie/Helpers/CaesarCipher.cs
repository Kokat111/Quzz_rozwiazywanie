using System.Text;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Input;

namespace Quzz_rozwiazywanie.Helpers
{
    public static class CaesarCipher
    {
        private const string Alphabet = "aąbcćdeęfghijklłmnńoópqrsśtuvwxyzźżABCDEFGHIJKLMNOPQRSTUVWXYZĄĆĘŁŃÓŚŹŻ";

        public static string Encrypt(string input, int shift)
        {
            var sb = new StringBuilder();
            foreach (char c in input)
            {
                int index = Alphabet.IndexOf(c);
                if (index >= 0)
                {
                    int newIndex = (index + shift) % Alphabet.Length;
                    sb.Append(Alphabet[newIndex]);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string Decrypt(string input, int shift)
        {
            char[] textToArray = input.ToCharArray();
            string outputText = string.Empty;

            for (int i = 0; i < textToArray.Length; i++)
            {
                outputText += (char)(textToArray[i] - shift);
            }
            return outputText;
        }
    }
}