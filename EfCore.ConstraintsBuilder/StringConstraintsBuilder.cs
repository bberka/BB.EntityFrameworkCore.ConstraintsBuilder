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

  public StringConstraintsBuilder<TEntity> EmailAddress() => EmailAddress(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "EmailAddress"));

  public StringConstraintsBuilder<TEntity> EmailAddress(string constraintName) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '{InternalTool.EmailRegex}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> Url() => Url(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "Url"));

  public StringConstraintsBuilder<TEntity> Url(string constraintName) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '{InternalTool.UrlRegex}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> PhoneNumber() => PhoneNumber(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "PhoneNumber"));

  public StringConstraintsBuilder<TEntity> PhoneNumber(string constraintName) {
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '{InternalTool.PhoneNumberRegex}'"));
    return this;
  }


  public StringConstraintsBuilder<TEntity> RegexExpression(string regex) => RegexExpression(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "RegexExpression", regex), regex);

  public StringConstraintsBuilder<TEntity> RegexExpression(string constraintName, string regex) {
    var isValidRegex = InternalTool.IsValidRegex(regex);
    if (!isValidRegex) {
      throw new ArgumentException("Invalid regex expression", nameof(regex));
    }

    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '{regex}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> MinLength(int minLength) => MinLength(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "MinLength", minLength), minLength);

  public StringConstraintsBuilder<TEntity> MinLength(string constraintName, int minLength) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"LEN([{_columnName}]) >= {minLength}"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> MaxLength(int maxLength) => MinLength(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "MaxLength", maxLength), maxLength);

  public StringConstraintsBuilder<TEntity> MaxLength(string constraintName, int maxLength) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"LEN([{_columnName}]) <= {maxLength}"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> LengthBetween(int minLength, int maxLength) => LengthBetween(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "LengthBetween", minLength + "_" + maxLength), minLength, maxLength);

  public StringConstraintsBuilder<TEntity> LengthBetween(string constraintName, int minLength, int maxLength) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"LEN([{_columnName}]) >= {minLength} AND LEN([{_columnName}]) <= {maxLength}"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> EqualsOneOf(IEnumerable<string> acceptedValues) => EqualsOneOf(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "EqualsOneOf", acceptedValues), acceptedValues);

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

  public StringConstraintsBuilder<TEntity> NotEqualsOneOf(IEnumerable<string> acceptedValues) => NotEqualsOneOf(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "NotEqualsOneOf", acceptedValues), acceptedValues);

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

  public StringConstraintsBuilder<TEntity> Equals(string value) => Equals(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "Equals", value), value);

  public StringConstraintsBuilder<TEntity> Equals(string constraintName, string value) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] = '{value}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> NotEquals(string value) => NotEquals(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "NotEquals", value), value);

  public StringConstraintsBuilder<TEntity> NotEquals(string constraintName, string value) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] != '{value}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> StartsWith(string value) => StartsWith(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "StartsWith", value), value);

  public StringConstraintsBuilder<TEntity> StartsWith(string constraintName, string value) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '{value}%'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> EndsWith(string value) => EndsWith(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "EndsWith", value), value);

  public StringConstraintsBuilder<TEntity> EndsWith(string constraintName, string value) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '%{value}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> Contains(string value) => Contains(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "Contains", value), value);

  public StringConstraintsBuilder<TEntity> Contains(string constraintName, string value) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] LIKE '%{value}%'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> NotContains(string value) => NotContains(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "NotContains", value), value);

  public StringConstraintsBuilder<TEntity> NotContains(string constraintName, string value) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] NOT LIKE '%{value}%'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> Empty() => Empty(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "Empty"));

  public StringConstraintsBuilder<TEntity> Empty(string constraintName) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));

    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"LEN([{_columnName}]) = 0"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> NotEmpty() => NotEmpty(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "NotEmpty"));

  public StringConstraintsBuilder<TEntity> NotEmpty(string constraintName) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"LEN([{_columnName}]) > 0"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> NullOrWhiteSpace() => NullOrWhiteSpace(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "NullOrWhiteSpace"));

  public StringConstraintsBuilder<TEntity> NullOrWhiteSpace(string constraintName) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"TRIM([{_columnName}]) = ''"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> NotNullOrWhiteSpace() => NotNullOrWhiteSpace(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "NotNullOrWhiteSpace"));

  public StringConstraintsBuilder<TEntity> NotNullOrWhiteSpace(string constraintName) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"TRIM([{_columnName}]) != ''"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> EqualsProperty(Expression<Func<TEntity, string>> propertySelector) => EqualsProperty(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "EqualsProperty", propertySelector), propertySelector);

  public StringConstraintsBuilder<TEntity> EqualsProperty(string constraintName, Expression<Func<TEntity, string>> propertySelector) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    var propertyInfo = propertySelector.GetPropertyAccess();
    var propertyName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] != [{propertyName}]"));
    return this;
  }
  
  public StringConstraintsBuilder<TEntity> NotEqualsProperty(Expression<Func<TEntity, string>> propertySelector) => NotEqualsProperty(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "NotEqualsProperty", propertySelector), propertySelector);
  
  public StringConstraintsBuilder<TEntity> NotEqualsProperty(string constraintName, Expression<Func<TEntity, string>> propertySelector) {
    if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
    var propertyInfo = propertySelector.GetPropertyAccess();
    var propertyName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] = [{propertyName}]"));
    return this;
  }
  
}