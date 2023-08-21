
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCore.ConstraintsBuilder;


public sealed class ShortConstraintsBuilder<TEntity> where TEntity : class
{
  
  private readonly EntityTypeBuilder<TEntity> _builder;
  private readonly SqlServerProvider _serverProvider;

  private readonly string _columnName;
  private readonly string _tableName;
  internal ShortConstraintsBuilder(
    EntityTypeBuilder<TEntity> builder,
    PropertyInfo propertyInfo,
    SqlServerProvider serverProvider) {
    var isDataTypeMatch = propertyInfo.PropertyType == typeof(short) ||
                          propertyInfo.PropertyType == typeof(short?);
    if (!isDataTypeMatch) {
      throw new ArgumentException("Property type is not short. PropertyName: " + propertyInfo.Name, nameof(propertyInfo));
    }
    _builder = builder;
    _serverProvider = serverProvider;
    _tableName = _builder.Metadata.GetTableName() ?? typeof(TEntity).Name;
    _columnName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
  }
  
  public ShortConstraintsBuilder<TEntity> NumberInBetween(short min, short max)  => NumberInBetween(_builder.CreateUniqueConstraintName(_columnName, nameof(NumberInBetween)), min, max);
  public ShortConstraintsBuilder<TEntity> NumberInBetween(string uniqueConstraintName, short min, short max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= {min} AND [{_columnName}] <= {max}"));
    return this;
  }
  public ShortConstraintsBuilder<TEntity> NumberMin(short min)  => NumberMin(_builder.CreateUniqueConstraintName(_columnName, nameof(NumberMin)), min);
  public ShortConstraintsBuilder<TEntity> NumberMin(string uniqueConstraintName, short min) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= {min} "));
    return this;
  }

  
  public ShortConstraintsBuilder<TEntity> NumberMax(short max)  => NumberMax(_builder.CreateUniqueConstraintName(_columnName, nameof(NumberMax)), max);
  public ShortConstraintsBuilder<TEntity> NumberMax(string uniqueConstraintName, short max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <= {max}"));
    return this;
  }
  
  public ShortConstraintsBuilder<TEntity> EqualOneOf(IEnumerable<short> acceptedValues)  => EqualOneOf(_builder.CreateUniqueConstraintName(_columnName, nameof(EqualOneOf)), acceptedValues);
  public ShortConstraintsBuilder<TEntity> EqualOneOf(string uniqueConstraintName, IEnumerable<short> acceptedValues) {
    var values = string.Join(',', acceptedValues);
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] IN ({values})"));
    return this;
  }
}