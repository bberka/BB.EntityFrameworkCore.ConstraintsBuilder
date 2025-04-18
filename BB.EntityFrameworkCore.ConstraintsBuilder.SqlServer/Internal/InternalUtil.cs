using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB.EntityFrameworkCore.ConstraintsBuilder.SqlServer.Internal;

internal static class InternalUtil
{
  public const string EmailRegex = "^[^@]+@[^@]+$";
  public const string UrlRegex = @"^(http://|https://|ftp://)";
  public const string PhoneNumberRegex = @"^(\+?1)?[2-9]\d{2}[2-9](?!11)\d{6}$";
  public const string CreditCardRegex = @"^(\d{4}[- ]){3}\d{4}|\d{16}$";
  
  
  private static string CreateUniqueConstraintName(string tableName, string columnName, string suffix, int count) {
    var sb = new StringBuilder();
    sb.Append("CK_");
    sb.Append(tableName);
    sb.Append('_');
    sb.Append(columnName);
    sb.Append('_');
    sb.Append(suffix);
    sb.Append('_');
    sb.Append(count);
    return sb.ToString();
  }

  internal static string CreateUniqueConstraintName<T>(this EntityTypeBuilder<T> builder,
                                                       string columnName, 
                                                       string suffix) where T : class{
    var tableName = builder.Metadata.GetTableName() ?? typeof(T).Name;
    var count = builder.Metadata.GetCheckConstraints().Count(x => x.Name?.Contains(columnName) == true);
    return CreateUniqueConstraintName(tableName, columnName, suffix, count);
  }

  internal static bool IsValidRegex(string regex) {
    try {
      _ = new Regex(regex);
      return true;
    }
    catch (ArgumentException) {
      return false;
    }
  }
}