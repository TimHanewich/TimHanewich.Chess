using System.Text;

namespace TimHanewich.Chess.Extensions
{
    public static class StringExtensions
    {
        public static string NormalizeSpaces(this string input)
        {
            var output = new StringBuilder();
            bool isSpace = false;

            foreach (char c in input)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (!isSpace)
                    {
                        output.Append(' ');
                        isSpace = true;
                    }
                }
                else
                {
                    output.Append(c);
                    isSpace = false;
                }
            }

            return output.ToString();
        }
    }
}