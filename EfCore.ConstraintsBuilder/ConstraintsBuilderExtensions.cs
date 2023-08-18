using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCore.ConstraintsBuilder;

public static class ConstraintsBuilderExtensions
{
  public static StringConstraintsBuilder<TEntity> AddConstraintsFor<TEntity>(
    this EntityTypeBuilder<TEntity> builder,
    Expression<Func<TEntity, string>> keySelector,
    SqlServerProvider serverProvider = SqlServerProvider.SqlServer)
    where TEntity : class {
    return new StringConstraintsBuilder<TEntity>(builder, keySelector.GetPropertyAccess(), serverProvider);
  }
  public static IntConstraintsBuilder<TEntity> AddConstraintsFor<TEntity>(
    this EntityTypeBuilder<TEntity> builder,
    Expression<Func<TEntity, int>> keySelector,
    SqlServerProvider serverProvider = SqlServerProvider.SqlServer)
    where TEntity : class {
    return new IntConstraintsBuilder<TEntity>(builder, keySelector.GetPropertyAccess(), serverProvider);
  }
  
  public static LongConstraintsBuilder<TEntity> AddConstraintsFor<TEntity>(
    this EntityTypeBuilder<TEntity> builder,
    Expression<Func<TEntity, long>> keySelector,
    SqlServerProvider serverProvider = SqlServerProvider.SqlServer)
    where TEntity : class {
    return new LongConstraintsBuilder<TEntity>(builder, keySelector.GetPropertyAccess(), serverProvider);
  }
  
  
  // public static void AddConstraintsFromDataAnnotations<TEntity>(this EntityTypeBuilder<TEntity> builder,
  //                                                               SupportedConstraintServerType serverType = SupportedConstraintServerType.SqlServer) where TEntity : class {
  //   var properties = typeof(TEntity).GetProperties();
  //   var rangeAttribute = typeof(RangeAttribute);
  //   var stringLengthAttribute = typeof(StringLengthAttribute);
  //   var maxLengthAttribute = typeof(MaxLengthAttribute);
  //   var minLengthAttribute = typeof(MinLengthAttribute);
  //   var phoneAttribute = typeof(PhoneAttribute);
  //   var emailAttribute = typeof(EmailAddressAttribute);
  //   var creditCardAttribute = typeof(CreditCardAttribute);
  //   var urlAttribute = typeof(UrlAttribute);
  //   var regexAttribute = typeof(RegularExpressionAttribute);
  //   foreach (var prop in properties) {
  //     var range = prop.GetCustomAttributes(rangeAttribute, false).FirstOrDefault() as RangeAttribute;
  //     var stringLength = prop.GetCustomAttributes(stringLengthAttribute, false).FirstOrDefault() as StringLengthAttribute;
  //     var maxLength = prop.GetCustomAttributes(maxLengthAttribute, false).FirstOrDefault() as MaxLengthAttribute;
  //     var minLength = prop.GetCustomAttributes(minLengthAttribute, false).FirstOrDefault() as MinLengthAttribute;
  //     var phone = prop.GetCustomAttributes(phoneAttribute, false).FirstOrDefault() as PhoneAttribute;
  //     var email = prop.GetCustomAttributes(emailAttribute, false).FirstOrDefault() as EmailAddressAttribute;
  //     var creditCard = prop.GetCustomAttributes(creditCardAttribute, false).FirstOrDefault() as CreditCardAttribute;
  //     var url = prop.GetCustomAttributes(urlAttribute, false).FirstOrDefault() as UrlAttribute;
  //     var regex = prop.GetCustomAttributes(regexAttribute, false).FirstOrDefault() as RegularExpressionAttribute;
  //     if (range != null) {
  //       builder.AddConstraintsFor(prop.Name, serverType)
  //              .NumberInBetween("CK_" + prop.Name + "_Range", range.Minimum, range.Maximum);
  //     }
  //
  //     if (stringLength != null) {
  //       builder.AddConstraintsFor(prop.Name, serverType)
  //              .StringLengthBetween("CK_" + prop.Name + "_StringLength", stringLength.MinimumLength, stringLength.MaximumLength);
  //     }
  //
  //     if (maxLength != null) {
  //       builder.AddConstraintsFor(prop.Name, serverType)
  //              .StringMaxLength("CK_" + prop.Name + "_MaxLength", maxLength.Length);
  //     }
  //
  //     if (minLength != null) {
  //       builder.AddConstraintsFor(prop.Name, serverType)
  //              .StringMinLength("CK_" + prop.Name + "_MinLength", minLength.Length);
  //     }
  //
  //     if (phone != null) {
  //       builder.AddConstraintsFor(prop.Name, serverType)
  //              .PhoneNumber("CK_" + prop.Name + "_PhoneNumber");
  //     }
  //
  //     if (email != null) {
  //       builder.AddConstraintsFor(prop.Name, serverType)
  //              .EmailAddress("CK_" + prop.Name + "_EmailAddress");
  //     }
  //
  //     if (creditCard != null) {
  //       builder.AddConstraintsFor(prop.Name, serverType)
  //              .CreditCard("CK_" + prop.Name + "_CreditCard");
  //     }
  //
  //     if (url != null) {
  //       builder.AddConstraintsFor(prop.Name, serverType)
  //              .StringUrl("CK_" + prop.Name + "_Url");
  //     }
  //
  //     if (regex != null) {
  //       builder.AddConstraintsFor(prop.Name, serverType)
  //              .RegexExpression("CK_" + prop.Name + "_Regex", regex.Pattern);
  //     }
  //   }
  // }
}