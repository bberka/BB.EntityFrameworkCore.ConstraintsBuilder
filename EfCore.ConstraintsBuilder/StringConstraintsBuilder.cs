using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
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

  public StringConstraintsBuilder<TEntity> EmailAddress() => EmailAddress(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "EmailAddress"));

  public StringConstraintsBuilder<TEntity> EmailAddress(string constraintName) {
    constraintName = string.IsNullOrEmpty(constraintName) 
                             ? InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "EmailAddress") 
                             : constraintName;
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"\"{_columnName}\" ~ '{InternalTool.EmailRegex}'"));
    return this;
  }
  public StringConstraintsBuilder<TEntity> Url() => Url(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "Url"));
  public StringConstraintsBuilder<TEntity> Url(string constraintName) {
    constraintName = string.IsNullOrEmpty(constraintName) 
                             ? InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "Url") 
                             : constraintName;
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"\"{_columnName}\" ~ '{InternalTool.UrlRegex}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> PhoneNumber() => PhoneNumber(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "PhoneNumber"));

  public StringConstraintsBuilder<TEntity> PhoneNumber(string constraintName) {
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"\"{_columnName}\" ~ '{InternalTool.PhoneNumberRegex}'"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> CreditCard() => CreditCard(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "CreditCard"));

  public StringConstraintsBuilder<TEntity> CreditCard(string constraintName) {
    constraintName = string.IsNullOrEmpty(constraintName) 
                             ? InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "CreditCard") 
                             : constraintName;
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"\"{_columnName}\" ~ '{InternalTool.CreditCardRegex}'"));
    return this;
  }
  public StringConstraintsBuilder<TEntity> RegexExpression(string regex) => RegexExpression(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "RegexExpression"), regex);

  public StringConstraintsBuilder<TEntity> RegexExpression(string constraintName, string regex) {
    var isValidRegex = InternalTool.IsValidRegex(regex);
    if (!isValidRegex) {
      throw new ArgumentException("Invalid regex expression", nameof(regex));
    }
    constraintName = string.IsNullOrEmpty(constraintName) 
                             ? InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "RegexExpression") 
                             : constraintName;
    _builder.ToTable(x => x.HasCheckConstraint(constraintName, $"\"{_columnName}\" ~ '{regex}'"));
    return this;
  }
  
  public StringConstraintsBuilder<TEntity> MinLength(int minLength) => MinLength(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "MinLength"), minLength);

  public StringConstraintsBuilder<TEntity> MinLength(string customConstraintName, int minLength) {
    customConstraintName = string.IsNullOrEmpty(customConstraintName) 
                             ? InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "MinLength") 
                             : customConstraintName;
    _builder.ToTable(x => x.HasCheckConstraint(customConstraintName, $"LENGTH(\"{_columnName}\") >= {minLength}"));
    return this;
  }
  public StringConstraintsBuilder<TEntity> MaxLength(int maxLength) => MinLength(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "MaxLength"), maxLength);

  public StringConstraintsBuilder<TEntity> MaxLength(string customConstraintName, int maxLength) {
    customConstraintName = string.IsNullOrEmpty(customConstraintName) 
                             ? InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "MaxLength") 
                             : customConstraintName;
    _builder.ToTable(x => x.HasCheckConstraint(customConstraintName, $"LENGTH(\"{_columnName}\") <= {maxLength}"));
    return this;
  }

  public StringConstraintsBuilder<TEntity> LengthBetween(int minLength, int maxLength) => LengthBetween(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "LengthBetween"), minLength, maxLength);

  public StringConstraintsBuilder<TEntity> LengthBetween(string customConstraintName, int minLength, int maxLength) {
    customConstraintName = string.IsNullOrEmpty(customConstraintName) 
                             ? InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "LengthBetween") 
                             : customConstraintName;
    _builder.ToTable(x => x.HasCheckConstraint(customConstraintName, $"LENGTH(\"{_columnName}\") >= {minLength} AND LENGTH(\"{_columnName}\") <= {maxLength}"));
    return this;
  }
  
  public StringConstraintsBuilder<TEntity> EqualOneOf(IEnumerable<string> acceptedValues) => EqualOneOf(InternalTool.CreateUniqueConstraintName(_tableName, _columnName, "EqualOneOf"), acceptedValues);
  public StringConstraintsBuilder<TEntity> EqualOneOf(string customConstraintName,IEnumerable<string> acceptedValues) {
    customConstraintName = string.IsNullOrEmpty(customConstraintName) 
                             ? InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "EqualOneOf") 
                             : customConstraintName;
    var sb = new StringBuilder();
    foreach (var value in acceptedValues) {
      sb.Append($"'{value}',");
    }
    var values = sb.ToString().TrimEnd(',');
    _builder.ToTable(x => x.HasCheckConstraint(customConstraintName, $"\"{_columnName}\" IN ({values})"));
    return this;
  }



}