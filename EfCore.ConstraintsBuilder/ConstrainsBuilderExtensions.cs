using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfCore.ConstraintsBuilder;

public static class ConstrainsBuilderExtensions
{
  
  /// <summary>
  /// Adds constraint builder to current entity type builder
  /// </summary>
  /// <param name="builder"></param>
  /// <param name="keySelector"></param>
  /// <param name="serverType"></param>
  /// <typeparam name="TEntity"></typeparam>
  /// <typeparam name="TProperty"></typeparam>
  /// <returns></returns>
  public static ConstraintsBuilder<TEntity> AddConstraintsFor<TEntity,TProperty>(this EntityTypeBuilder<TEntity> builder, 
                                                                                 Expression<Func<TEntity, TProperty>> keySelector, 
                                                                                 SupportedConstraintServerType serverType = SupportedConstraintServerType.SqlServer) where TEntity : class
  {
    return new ConstraintsBuilder<TEntity>(builder, keySelector.GetPropertyAccess().Name, serverType);
  }
}