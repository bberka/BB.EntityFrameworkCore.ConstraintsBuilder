using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCore.ConstraintsBuilder;

public sealed class DateTimeConstraintsBuilder<TEntity> where TEntity : class
{
  private readonly EntityTypeBuilder<TEntity> _builder;
  private readonly SqlServerProvider _serverProvider;

  private readonly string _columnName;
  private readonly string _tableName;

  internal DateTimeConstraintsBuilder(
    EntityTypeBuilder<TEntity> builder,
    PropertyInfo propertyInfo,
    SqlServerProvider serverProvider) {
    var isDataTypeMatch = propertyInfo.PropertyType == typeof(DateTime) ||
                          propertyInfo.PropertyType == typeof(DateTime?);
    if (!isDataTypeMatch) {
      throw new ArgumentException("Property type is not datetime. PropertyName: " + propertyInfo.Name, nameof(propertyInfo));
    }

    _builder = builder;
    _serverProvider = serverProvider;
    _tableName = _builder.Metadata.GetTableName() ?? typeof(TEntity).Name;
    _columnName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
  }
  
  public DateTimeConstraintsBuilder<TEntity> DateInBetween(DateTime min, DateTime max)  => DateInBetween(_builder.CreateUniqueConstraintName(_columnName, nameof(DateInBetween)), min, max);
  
  public DateTimeConstraintsBuilder<TEntity> DateInBetween(string uniqueConstraintName, DateTime min, DateTime max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= '{min:yyyy-MM-dd}' AND [{_columnName}] <= '{max:yyyy-MM-dd}'"));
    return this;
  }
  
  public DateTimeConstraintsBuilder<TEntity> DateMin(DateTime min)  => DateMin(_builder.CreateUniqueConstraintName(_columnName, nameof(DateMin)), min);
  
  public DateTimeConstraintsBuilder<TEntity> DateMin(string uniqueConstraintName, DateTime min) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= '{min:yyyy-MM-dd}'"));
    return this;
  }
  
  public DateTimeConstraintsBuilder<TEntity> DateMax(DateTime max)  => DateMax(_builder.CreateUniqueConstraintName(_columnName, nameof(DateMax)), max);
  
  public DateTimeConstraintsBuilder<TEntity> DateMax(string uniqueConstraintName, DateTime max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <= '{max:yyyy-MM-dd}'"));
    return this;
  }
  
  public DateTimeConstraintsBuilder<TEntity> TimeInBetween(TimeSpan min, TimeSpan max)  => TimeInBetween(_builder.CreateUniqueConstraintName(_columnName, nameof(TimeInBetween)), min, max);
  
  public DateTimeConstraintsBuilder<TEntity> TimeInBetween(string uniqueConstraintName, TimeSpan min, TimeSpan max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= '{min:hh\\:mm\\:ss}' AND [{_columnName}] <= '{max:hh\\:mm\\:ss}'"));
    return this;
  }
  
  public DateTimeConstraintsBuilder<TEntity> TimeMin(TimeSpan min)  => TimeMin(_builder.CreateUniqueConstraintName(_columnName, nameof(TimeMin)), min);
  
  public DateTimeConstraintsBuilder<TEntity> TimeMin(string uniqueConstraintName, TimeSpan min) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= '{min:hh\\:mm\\:ss}'"));
    return this;
  }
  
  public DateTimeConstraintsBuilder<TEntity> TimeMax(TimeSpan max)  => TimeMax(_builder.CreateUniqueConstraintName(_columnName, nameof(TimeMax)), max);
  
  public DateTimeConstraintsBuilder<TEntity> TimeMax(string uniqueConstraintName, TimeSpan max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <= '{max:hh\\:mm\\:ss}'"));
    return this;
  }
  
  
}