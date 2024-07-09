namespace CBP.Services.Extensions
{
    // TODO: Class extensions for widely used functions
    public class SringExtensions
    {
        public bool StartsWithLetter(this string input, char letter)
        {
            return input.StartsWithLetter(letter.ToString());
        }

        public bool StartsWithLetter(this string input, string letter)
        {
            return input.StartsWith(letter);
        }
    }
}
