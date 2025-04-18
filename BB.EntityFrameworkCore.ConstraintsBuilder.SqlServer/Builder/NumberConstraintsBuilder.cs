using System.Numerics;
using System.Reflection;
using BB.EntityFrameworkCore.ConstraintsBuilder.SqlServer.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB.EntityFrameworkCore.ConstraintsBuilder.SqlServer.Builder;

public sealed class NumberConstraintsBuilder<TEntity, TProperty> where TEntity : class where TProperty : INumber<TProperty>
{
	private readonly EntityTypeBuilder<TEntity> _builder;

	private readonly string _columnName;
	private readonly string _tableName;

	internal NumberConstraintsBuilder(
		EntityTypeBuilder<TEntity> builder,
		PropertyInfo propertyInfo) {
		_builder = builder;
		_tableName = _builder.Metadata.GetTableName() ?? typeof(TEntity).Name;
		_columnName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
	}

	public NumberConstraintsBuilder<TEntity, TProperty> NumberInBetween(TProperty min, TProperty max) {
		return NumberInBetween(_builder.CreateUniqueConstraintName(_columnName, nameof(NumberInBetween)), min, max);
	}

	public NumberConstraintsBuilder<TEntity, TProperty> NumberInBetween(string uniqueConstraintName, TProperty min, TProperty max) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= {min} AND [{_columnName}] <= {max}"));
		return this;
	}

	public NumberConstraintsBuilder<TEntity, TProperty> NumberMin(TProperty min) {
		return NumberMin(_builder.CreateUniqueConstraintName(_columnName, nameof(NumberMin)), min);
	}

	public NumberConstraintsBuilder<TEntity, TProperty> NumberMin(string uniqueConstraintName, TProperty min) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= {min} "));
		return this;
	}

	public NumberConstraintsBuilder<TEntity, TProperty> NumberMax(TProperty max) {
		return NumberMax(_builder.CreateUniqueConstraintName(_columnName, nameof(NumberMax)), max);
	}

	public NumberConstraintsBuilder<TEntity, TProperty> NumberMax(string uniqueConstraintName, TProperty max) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <= {max}"));
		return this;
	}

	public NumberConstraintsBuilder<TEntity, TProperty> EqualOneOf(IEnumerable<TProperty?> acceptedValues) {
		return EqualOneOf(_builder.CreateUniqueConstraintName(_columnName, nameof(EqualOneOf)), acceptedValues);
	}

	public NumberConstraintsBuilder<TEntity, TProperty> EqualOneOf(string uniqueConstraintName, IEnumerable<TProperty?> acceptedValues) {
		var values = string.Join(',', acceptedValues);
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] IN ({values})"));
		return this;
	}
}