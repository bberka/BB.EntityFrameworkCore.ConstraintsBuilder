

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCore.ConstraintsBuilder;


public sealed class ByteConstraintsBuilder<TEntity> where TEntity : class
{
  
  private readonly EntityTypeBuilder<TEntity> _builder;
  private readonly SqlServerProvider _serverProvider;

  private readonly string _columnName;
  private readonly string _tableName;
  internal ByteConstraintsBuilder(
    EntityTypeBuilder<TEntity> builder,
    PropertyInfo propertyInfo,
    SqlServerProvider serverProvider) {
    var isDataTypeMatch = propertyInfo.PropertyType == typeof(byte) ||
                          propertyInfo.PropertyType == typeof(byte?);
    if (!isDataTypeMatch) {
      throw new ArgumentException("Property type is not byte. PropertyName: " + propertyInfo.Name, nameof(propertyInfo));
    }
    _builder = builder;
    _serverProvider = serverProvider;
    _tableName = _builder.Metadata.GetTableName() ?? typeof(TEntity).Name;
    _columnName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
  }
  
  public ByteConstraintsBuilder<TEntity> NumberInBetween(byte min, byte max)  => NumberInBetween(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "NumberInBetween"), min, max);
  public ByteConstraintsBuilder<TEntity> NumberInBetween(string uniqueConstraintName, byte min, byte max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" >= {min} AND \"{_columnName}\" <= {max}"));
    return this;
  }
  public ByteConstraintsBuilder<TEntity> NumberMin(byte min)  => NumberMin(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "NumberMin"), min);
  public ByteConstraintsBuilder<TEntity> NumberMin(string uniqueConstraintName, byte min) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" >= {min} "));
    return this;
  }

  
  public ByteConstraintsBuilder<TEntity> NumberMax(byte max)  => NumberMax(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "NumberMax"), max);
  public ByteConstraintsBuilder<TEntity> NumberMax(string uniqueConstraintName, byte max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" <= {max}"));
    return this;
  }
  
  public ByteConstraintsBuilder<TEntity> EqualOneOf(IEnumerable<byte> acceptedValues)  => EqualOneOf(InternalTool.CreateUniqueConstraintName(_tableName,_columnName, "EqualOneOf"), acceptedValues);
  public ByteConstraintsBuilder<TEntity> EqualOneOf(string uniqueConstraintName, IEnumerable<byte> acceptedValues) {
    var values = string.Join(',', acceptedValues);
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" IN ({values})"));
    return this;
  }
}