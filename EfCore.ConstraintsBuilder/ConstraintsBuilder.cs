using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCore.ConstraintsBuilder;

public sealed class ConstraintsBuilder<TEntity> where TEntity : class
{
  private readonly EntityTypeBuilder<TEntity> _builder;

  private readonly string _columnName;
  private readonly SupportedConstraintServerType _serverType;

  
  /// <summary>
  /// Builder for constraints on a column
  /// </summary>
  /// <param name="builder"></param>
  /// <param name="columnName"></param>
  /// <param name="serverType"></param>
  internal ConstraintsBuilder(EntityTypeBuilder<TEntity> builder, string columnName, SupportedConstraintServerType serverType) {
    _builder = builder;
    _columnName = columnName;
    _serverType = serverType;
  }

  
  public ConstraintsBuilder<TEntity> RegexExpression( string uniqueConstraintName,string regex) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" ~ '{regex}'"));
    return this;
  }
  public ConstraintsBuilder<TEntity> NumberInBetween(string uniqueConstraintName, int min, int max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" >= {min} AND \"{_columnName}\" <= {max}"));
    return this;
  }
  public ConstraintsBuilder<TEntity> NumberMin(string uniqueConstraintName,int min) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" >= {min} "));
    return this;
  }
  public ConstraintsBuilder<TEntity> NumberMax(string uniqueConstraintName,int max) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" <= {max}"));
    return this;
  }
  public ConstraintsBuilder<TEntity> StringMinLength(string uniqueConstraintName,int minLength) {
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"LENGTH(\"{_columnName}\") >= {minLength}"));
    return this;
  }
  
  public ConstraintsBuilder<TEntity> StringEqualOneOf(string uniqueConstraintName,params string[] acceptedValues) {
    var values = string.Join(',', acceptedValues);
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" IN ({values})"));
    return this;
  }
  
  public ConstraintsBuilder<TEntity> NumberEqualOneOf(string uniqueConstraintName,params int[] acceptedValues) {
    var values = string.Join(',', acceptedValues);
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" IN ({values})"));
    return this;
  }
  
  public ConstraintsBuilder<TEntity> StringEmailAddress(string uniqueConstraintName) {
    const string EmailRegex =  "^[^@]+@[^@]+$";
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" ~ '{EmailRegex}'"));
    return this;
  }
  
  public ConstraintsBuilder<TEntity> StringUrl(string uniqueConstraintName) {
    const string UrlRegex =  @"^(http://|https://|ftp://)";
    _builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"\"{_columnName}\" ~ '{UrlRegex}'"));
    return this;
  }
}