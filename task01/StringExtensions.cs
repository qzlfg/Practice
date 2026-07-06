using System.Text;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        input = input.ToLower();
        StringBuilder sb = new StringBuilder();

        foreach (char symbol in input)
        {
            if (!char.IsWhiteSpace(symbol) && !char.IsPunctuation(symbol))
            {
                sb.Append(symbol);
            }
        }

        input = sb.ToString();

        if (string.IsNullOrEmpty(input)) return false;

        int left = 0;
        int right = input.Length - 1;

        while (left < right)
        {
            if (input[left] != input[right])
            {
                return false;
            }
            left++;
            right--;
        }
        return true;
    }
}