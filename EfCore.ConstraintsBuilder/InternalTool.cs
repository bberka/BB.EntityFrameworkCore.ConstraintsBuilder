using System.Text;
using System.Text.RegularExpressions;

namespace EfCore.ConstraintsBuilder;

public static class InternalTool
{
  public const string EmailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
  public const string UrlRegex = @"^(?!mailto:)(?:(?:http|https|ftp)://)(?:\\S+(?::\\S*)?@)?(?:(?:(?:[1-9]\\d?|1\\d\\d|2[01]\\d|22[0-3])(?:\\.(?:1?\\d{1,2}|2[0-4]\\d|25[0-5])){2}(?:\\.(?:[0-9]\\d?|1\\d\\d|2[0-4]\\d|25[0-4]))|(?:(?:[a-z\\u00a1-\\uffff0-9]+-?)*[a-z\\u00a1-\\uffff0-9]+)(?:\\.(?:[a-z\\u00a1-\\uffff0-9]+-?)*[a-z\\u00a1-\\uffff0-9]+)*(?:\\.(?:[a-z\\u00a1-\\uffff]{2,})))|localhost)(?::\\d{2,5})?(?:(/|\\?|#)[^\\s]*)?$";
  public const string PhoneNumberRegex = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$";


  public static string CreateUniqueConstraintName(string tableName, string columnName, string suffix, object? valueForCheck = null) {
    var sb = new StringBuilder();
    sb.Append("CK_");
    sb.Append(tableName);
    sb.Append('_');
    sb.Append(columnName);
    sb.Append('_');
    sb.Append(suffix);
    sb.Append('_');
    var num = valueForCheck?.GetHashCode() ?? 0;
    sb.Append(num);
    return sb.ToString();
  }

  public static bool IsValidRegex(string regex) {
    try {
      _ = new Regex(regex);
      return true;
    }
    catch (ArgumentException) {
      return false;
    }
  }
}