using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB.EntityFrameworkCore.ConstraintsBuilder.SqlServer;

public static class ConstraintsBuilderExtensions
{
  public static StringConstraintsBuilder<TEntity> AddConstraintsFor<TEntity>(
    this EntityTypeBuilder<TEntity> builder,
    Expression<Func<TEntity, string?>> keySelector)
    where TEntity : class {
    return new StringConstraintsBuilder<TEntity>(builder, keySelector.GetPropertyAccess());
  }
  public static IntConstraintsBuilder<TEntity> AddConstraintsFor<TEntity>(
    this EntityTypeBuilder<TEntity> builder,
    Expression<Func<TEntity, int?>> keySelector)
    where TEntity : class {
    return new IntConstraintsBuilder<TEntity>(builder, keySelector.GetPropertyAccess());
  }
  
  public static LongConstraintsBuilder<TEntity> AddConstraintsFor<TEntity>(
    this EntityTypeBuilder<TEntity> builder,
    Expression<Func<TEntity, long?>> keySelector)
    where TEntity : class {
    return new LongConstraintsBuilder<TEntity>(builder, keySelector.GetPropertyAccess());
  }
  
  public static ShortConstraintsBuilder<TEntity> AddConstraintsFor<TEntity>(
    this EntityTypeBuilder<TEntity> builder,
    Expression<Func<TEntity, short?>> keySelector)
    where TEntity : class {
    return new ShortConstraintsBuilder<TEntity>(builder, keySelector.GetPropertyAccess());
  }
  
  public static ByteConstraintsBuilder<TEntity> AddConstraintsFor<TEntity>(
    this EntityTypeBuilder<TEntity> builder,
    Expression<Func<TEntity, byte?>> keySelector)
    where TEntity : class {
    return new ByteConstraintsBuilder<TEntity>(builder, keySelector.GetPropertyAccess());
  }
  
  public static DateTimeConstraintsBuilder<TEntity> AddConstraintsFor<TEntity>(
    this EntityTypeBuilder<TEntity> builder,
    Expression<Func<TEntity, DateTime?>> keySelector)
    where TEntity : class {
    return new DateTimeConstraintsBuilder<TEntity>(builder, keySelector.GetPropertyAccess());
  }
}