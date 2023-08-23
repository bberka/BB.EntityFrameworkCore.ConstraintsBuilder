using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCore.ConstraintsBuilder;



public sealed class StringConstraintsBuilder<TEntity>  where TEntity : class
{
  private readonly EntityTypeBuilder<TEntity> _builder;
  private readonly SqlServerProvider _serverProvider;

  private readonly string _columnName;
  private readonly string _tableName;

  internal StringConstraintsBuilder(
    EntityTypeBuilder<TEntity> builder,
    PropertyInfo propertyInfo,
    SqlServerProvider serverProvider) {
    var isDataTypeMatch = propertyInfo.PropertyType == typeof(string);
    if (!isDataTypeMatch) {
      throw new ArgumentException("Property type is not string. PropertyName: " + propertyInfo.Name, nameof(propertyInfo));
    }
    _builder = builder;
    _serverProvider = serverProvider;
    _tableName = _builder.Metadata.GetTableName() ?? typeof(TEntity).Name;
    _columnName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
  }

  public StringConstraintsBuilder<TEntity> EmailAddress() => EmailAddress(_builder.CreateUniqueConstraintName(_columnName, nameof(EmailAddress)));

  public StringConstraintsBuilder<TEntity> EmailAddress(string constraintName) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '{InternalTool.EmailRegex}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> Url() => Url(_builder.CreateUniqueConstraintName(_columnName, nameof(Url)));

  public StringConstraintsBuilder<TEntity> Url(string constraintName) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '{InternalTool.UrlRegex}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> PhoneNumber() => PhoneNumber(_builder.CreateUniqueConstraintName(_columnName, nameof(PhoneNumber)));

  public StringConstraintsBuilder<TEntity> PhoneNumber(string constraintName) {
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '{InternalTool.PhoneNumberRegex}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> RegexExpression(string regex) => RegexExpression(_builder.CreateUniqueConstraintName(_columnName, nameof(RegexExpression)), regex);

  public StringConstraintsBuilder<TEntity> RegexExpression(string constraintName, string regex) {
    var isValidRegex = InternalTool.IsValidRegex(regex);
    if (!isValidRegex) {
      throw new ArgumentException("Invalid regex expression", nameof(regex));
    }

    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '{regex}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> MinLength(int minLength) => MinLength(_builder.CreateUniqueConstraintName(_columnName, nameof(MinLength)), minLength);

  public StringConstraintsBuilder<TEntity> MinLength(string constraintName, int minLength) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"LEN([{_columnName}]) >= {minLength}"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> MaxLength(int maxLength) => MinLength(_builder.CreateUniqueConstraintName(_columnName, nameof(MaxLength)), maxLength);

  public StringConstraintsBuilder<TEntity> MaxLength(string constraintName, int maxLength) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"LEN([{_columnName}]) <= {maxLength}"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> LengthBetween(int minLength, int maxLength) => LengthBetween(_builder.CreateUniqueConstraintName(_columnName, nameof(LengthBetween)), minLength, maxLength);

  public StringConstraintsBuilder<TEntity> LengthBetween(string constraintName, int minLength, int maxLength) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"LEN([{_columnName}]) >= {minLength} AND LEN([{_columnName}]) <= {maxLength}"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> EqualsOneOf(IEnumerable<string> acceptedValues) => EqualsOneOf(_builder.CreateUniqueConstraintName(_columnName, nameof(EqualsOneOf)), acceptedValues);

  public StringConstraintsBuilder<TEntity> EqualsOneOf(string constraintName, IEnumerable<string> acceptedValues) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    var sb = new StringBuilder();
    foreach (var value in acceptedValues) {
      sb.Append($"'{value}',");
    }

    var values = sb.ToString().TrimEnd(',');
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] IN ({values})"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> NotEqualsOneOf(IEnumerable<string> acceptedValues) => NotEqualsOneOf(_builder.CreateUniqueConstraintName(_columnName, nameof(NotEqualsOneOf)), acceptedValues);

  public StringConstraintsBuilder<TEntity> NotEqualsOneOf(string constraintName, IEnumerable<string> acceptedValues) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    var sb = new StringBuilder();
    foreach (var value in acceptedValues) {
      sb.Append($"'{value}',");
    }

    var values = sb.ToString().TrimEnd(',');
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] NOT IN ({values})"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> Equals(string value) => Equals(_builder.CreateUniqueConstraintName(_columnName, nameof(Equals)), value);

  public StringConstraintsBuilder<TEntity> Equals(string constraintName, string value) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] = '{value}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> NotEquals(string value) => NotEquals(_builder.CreateUniqueConstraintName(_columnName, nameof(NotEquals)), value);

  public StringConstraintsBuilder<TEntity> NotEquals(string constraintName, string value) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] != '{value}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> StartsWith(string value) => StartsWith(_builder.CreateUniqueConstraintName(_columnName, nameof(StartsWith)), value);

  public StringConstraintsBuilder<TEntity> StartsWith(string constraintName, string value) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '{value}%'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> EndsWith(string value) => EndsWith(_builder.CreateUniqueConstraintName(_columnName, nameof(EndsWith)), value);

  public StringConstraintsBuilder<TEntity> EndsWith(string constraintName, string value) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '%{value}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> Contains(string value) => Contains(_builder.CreateUniqueConstraintName(_columnName, nameof(Contains)), value);

  public StringConstraintsBuilder<TEntity> Contains(string constraintName, string value) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '%{value}%'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> NotContains(string value) => NotContains(_builder.CreateUniqueConstraintName(_columnName, nameof(NotContains)), value);

  public StringConstraintsBuilder<TEntity> NotContains(string constraintName, string value) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] NOT LIKE '%{value}%'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> Empty() => Empty(_builder.CreateUniqueConstraintName(_columnName, nameof(Empty)));

  public StringConstraintsBuilder<TEntity> Empty(string constraintName) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"LEN([{_columnName}]) = 0"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> NotEmpty() => NotEmpty(_builder.CreateUniqueConstraintName(_columnName, nameof(NotEmpty)));

  public StringConstraintsBuilder<TEntity> NotEmpty(string constraintName) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"LEN([{_columnName}]) > 0"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> NullOrWhiteSpace() => NullOrWhiteSpace(_builder.CreateUniqueConstraintName(_columnName, nameof(NullOrWhiteSpace)));

  public StringConstraintsBuilder<TEntity> NullOrWhiteSpace(string constraintName) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"TRIM([{_columnName}]) = ''"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> NotNullOrWhiteSpace() => NotNullOrWhiteSpace(_builder.CreateUniqueConstraintName(_columnName, nameof(NotNullOrWhiteSpace)));

  public StringConstraintsBuilder<TEntity> NotNullOrWhiteSpace(string constraintName) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"TRIM([{_columnName}]) != ''"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> EqualsProperty(Expression<Func<TEntity, string>> propertySelector) => EqualsProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(EqualsProperty)), propertySelector);

  public StringConstraintsBuilder<TEntity> EqualsProperty(string constraintName, Expression<Func<TEntity, string>> propertySelector) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    var propertyInfo = propertySelector.GetPropertyAccess();
    var propertyName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] != [{propertyName}]"));
    return this;
  }
  
  public StringConstraintsBuilder<TEntity> NotEqualsProperty(Expression<Func<TEntity, string>> propertySelector) => NotEqualsProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(NotEqualsProperty)), propertySelector);
  
  public StringConstraintsBuilder<TEntity> NotEqualsProperty(string constraintName, Expression<Func<TEntity, string>> propertySelector) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    var propertyInfo = propertySelector.GetPropertyAccess();
    var propertyName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] = [{propertyName}]"));
    return this;
  }
}