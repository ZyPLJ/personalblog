using System.Text.RegularExpressions;

namespace Personalblog.Migrate;

public static class CheckEmail
{
    public static bool CheckEmailFormat(string email)
    {
        string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        Regex regex = new Regex(emailPattern);
        return regex.IsMatch(email);
    }
}