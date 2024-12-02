using System.Text.RegularExpressions;

namespace Domain.Services;
public static class EmailValidator
{
    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public static bool IsValidEmail(string email)
    {
        return EmailRegex.IsMatch(email);
    }
}
