namespace CBP.Services.Extensions
{
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
