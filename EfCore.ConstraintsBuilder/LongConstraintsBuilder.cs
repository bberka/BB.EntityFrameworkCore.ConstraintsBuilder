using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCore.ConstraintsBuilder;

public sealed class LongConstraintsBuilder<TEntity> where TEntity : class
{
  
  private readonly EntityTypeBuilder<TEntity> _builder;
  private readonly SqlServerProvider _serverProvider;

  private readonly string _columnName;
  private readonly string _tableName;
  internal LongConstraintsBuilder(
    EntityTypeBuilder<TEntity> builder,
    PropertyInfo propertyInfo,
    SqlServerProvider serverProvider) {
    var isDataTypeMatch = propertyInfo.PropertyType == typeof(long) ||
                          propertyInfo.PropertyType == typeof(long?);
    if (!isDataTypeMatch) {
      throw new ArgumentException("Property type is not long. PropertyName: " + propertyInfo.Name, nameof(propertyInfo));
    }
    _builder = builder;
    _serverProvider = serverProvider;
    _tableName = _builder.Metadata.GetTableName() ?? typeof(TEntity).Name;
    _columnName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
  }
  
  public LongConstraintsBuilder<TEntity> NumberInBetween(long min, long max)  => NumberInBetween(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "NumberInBetween", min + "_" + max), min, max);
  public LongConstraintsBuilder<TEntity> NumberInBetween(string uniqueConstraintName, long min, long max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= {min} AND [{_columnName}] <= {max}"));
    return this;
  }
  public LongConstraintsBuilder<TEntity> NumberMin(long min)  => NumberMin(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "NumberMin", min), min);
  public LongConstraintsBuilder<TEntity> NumberMin(string uniqueConstraintName, long min) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= {min} "));
    return this;
  }

  
  public LongConstraintsBuilder<TEntity> NumberMax(long max)  => NumberMax(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "NumberMax", max), max);
  public LongConstraintsBuilder<TEntity> NumberMax(string uniqueConstraintName, long max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <= {max}"));
    return this;
  }
  
  public LongConstraintsBuilder<TEntity> EqualOneOf(IEnumerable<long> acceptedValues)  => EqualOneOf(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "EqualOneOf", acceptedValues), acceptedValues);
  public LongConstraintsBuilder<TEntity> EqualOneOf(string uniqueConstraintName, IEnumerable<long> acceptedValues) {
    var values = string.Join(',', acceptedValues);
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] IN ({values})"));
    return this;
  }
}