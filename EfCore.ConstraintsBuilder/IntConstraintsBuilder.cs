using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCore.ConstraintsBuilder;


public sealed class IntConstraintsBuilder<TEntity> where TEntity : class
{
  private readonly EntityTypeBuilder<TEntity> _builder;
  private readonly SqlServerProvider _serverProvider;

  private readonly string _columnName;
  private readonly string _tableName;
  internal IntConstraintsBuilder(
    EntityTypeBuilder<TEntity> builder,
    PropertyInfo propertyInfo,
    SqlServerProvider serverProvider) {
    var isDataTypeMatch = propertyInfo.PropertyType == typeof(int) ||
                          propertyInfo.PropertyType == typeof(int?);
    if (!isDataTypeMatch) {
      throw new ArgumentException("Property type is not int. PropertyName: " + propertyInfo.Name, nameof(propertyInfo));
    }
    _builder = builder;
    _serverProvider = serverProvider;
    _tableName = _builder.Metadata.GetTableName() ?? typeof(TEntity).Name;
    _columnName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
  }
  public IntConstraintsBuilder<TEntity> NumberInBetween(int min, int max)  => NumberInBetween(_builder.CreateUniqueConstraintName(_columnName, nameof(NumberInBetween)), min, max);
  public IntConstraintsBuilder<TEntity> NumberInBetween(string uniqueConstraintName, int min, int max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= {min} AND [{_columnName}] <= {max}"));
    return this;
  }
  public IntConstraintsBuilder<TEntity> NumberMin(int min)  => NumberMin(_builder.CreateUniqueConstraintName(_columnName, nameof(NumberMin)), min);
  public IntConstraintsBuilder<TEntity> NumberMin(string uniqueConstraintName, int min) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= {min} "));
    return this;
  }

  
  public IntConstraintsBuilder<TEntity> NumberMax(int max)  => NumberMax(_builder.CreateUniqueConstraintName(_columnName, nameof(NumberMax)), max);
  public IntConstraintsBuilder<TEntity> NumberMax(string uniqueConstraintName, int max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <= {max}"));
    return this;
  }
  
  public IntConstraintsBuilder<TEntity> EqualOneOf(IEnumerable<int> acceptedValues)  => EqualOneOf(_builder.CreateUniqueConstraintName(_columnName, nameof(EqualOneOf)), acceptedValues);
  public IntConstraintsBuilder<TEntity> EqualOneOf(string uniqueConstraintName, IEnumerable<int> acceptedValues) {
    var values = string.Join(',', acceptedValues);
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] IN ({values})"));
    return this;
  }
}