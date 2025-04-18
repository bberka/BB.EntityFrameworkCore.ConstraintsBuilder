using System.Linq.Expressions;
using System.Numerics;
using BB.EntityFrameworkCore.ConstraintsBuilder.SqlServer.Builder;
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

	public static NumberConstraintsBuilder<TEntity, TProperty> AddConstraintsFor<TEntity, TProperty>(
		this EntityTypeBuilder<TEntity> builder,
		Expression<Func<TEntity, TProperty?>> keySelector)
		where TEntity : class
		where TProperty : INumber<TProperty> {
		return new NumberConstraintsBuilder<TEntity, TProperty>(builder, keySelector.GetPropertyAccess());
	}

	public static DateTimeConstraintsBuilder<TEntity> AddConstraintsFor<TEntity>(
		this EntityTypeBuilder<TEntity> builder,
		Expression<Func<TEntity, DateTime?>> keySelector)
		where TEntity : class {
		return new DateTimeConstraintsBuilder<TEntity>(builder, keySelector.GetPropertyAccess());
	}

	public static GuidConstraintsBuilder<TEntity> AddConstraintsFor<TEntity>(
		this EntityTypeBuilder<TEntity> builder,
		Expression<Func<TEntity, Guid?>> keySelector)
		where TEntity : class {
		return new GuidConstraintsBuilder<TEntity>(builder, keySelector.GetPropertyAccess());
	}
}