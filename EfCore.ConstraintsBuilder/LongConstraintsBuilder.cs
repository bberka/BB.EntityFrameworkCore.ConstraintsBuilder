using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCore.ConstraintsBuilder;

public interface ILongConstraintsBuilder<TEntity> where TEntity : class
{
  LongConstraintsBuilder<TEntity> NumberInBetween(long min, long max);
  LongConstraintsBuilder<TEntity> NumberInBetween(string uniqueConstraintName, long min, long max);
  LongConstraintsBuilder<TEntity> NumberMin(long min);
  LongConstraintsBuilder<TEntity> NumberMin(string uniqueConstraintName, long min);
  LongConstraintsBuilder<TEntity> NumberMax(long max);
  LongConstraintsBuilder<TEntity> NumberMax(string uniqueConstraintName, long max);
  LongConstraintsBuilder<TEntity> EqualOneOf(IEnumerable<long> acceptedValues);
  LongConstraintsBuilder<TEntity> EqualOneOf(string uniqueConstraintName, IEnumerable<long> acceptedValues);
}

public sealed class LongConstraintsBuilder<TEntity> : ILongConstraintsBuilder<TEntity> where TEntity : class
{
  
  private readonly EntityTypeBuilder<TEntity> _builder;
  private readonly SqlServerProvider _serverProvider;

  private readonly string _columnName;
  private readonly string _tableName;
  internal LongConstraintsBuilder(
    EntityTypeBuilder<TEntity> builder,
    PropertyInfo propertyInfo,
    SqlServerProvider serverProvider) {
    _builder = builder;
    _serverProvider = serverProvider;
    _tableName = _builder.Metadata.GetTableName() ?? typeof(TEntity).Name;
    _columnName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
  }
  
  public LongConstraintsBuilder<TEntity> NumberInBetween(long min, long max)  => NumberInBetween(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "NumberInBetween"), min, max);
  public LongConstraintsBuilder<TEntity> NumberInBetween(string uniqueConstraintName, long min, long max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" >= {min} AND \"{_columnName}\" <= {max}"));
    return this;
  }
  public LongConstraintsBuilder<TEntity> NumberMin(long min)  => NumberMin(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "NumberMin"), min);
  public LongConstraintsBuilder<TEntity> NumberMin(string uniqueConstraintName, long min) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" >= {min} "));
    return this;
  }

  
  public LongConstraintsBuilder<TEntity> NumberMax(long max)  => NumberMax(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "NumberMax"), max);
  public LongConstraintsBuilder<TEntity> NumberMax(string uniqueConstraintName, long max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" <= {max}"));
    return this;
  }
  
  public LongConstraintsBuilder<TEntity> EqualOneOf(IEnumerable<long> acceptedValues)  => EqualOneOf(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "EqualOneOf"), acceptedValues);
  public LongConstraintsBuilder<TEntity> EqualOneOf(string uniqueConstraintName, IEnumerable<long> acceptedValues) {
    var values = string.Join(',', acceptedValues);
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" IN ({values})"));
    return this;
  }
}