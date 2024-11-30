using System.Text.RegularExpressions;

namespace Domain.Services;
internal static class EmailValidator
{
    private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    internal static bool IsValidEmail(string email)
    {
        return EmailRegex.IsMatch(email);
    }
}
