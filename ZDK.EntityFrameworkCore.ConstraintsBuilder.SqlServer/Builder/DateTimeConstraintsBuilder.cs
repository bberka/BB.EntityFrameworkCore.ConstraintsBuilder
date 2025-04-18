using System.Linq.Expressions;
using System.Reflection;
using BB.EntityFrameworkCore.ConstraintsBuilder.SqlServer.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BB.EntityFrameworkCore.ConstraintsBuilder.SqlServer.Builder;

public sealed class DateTimeConstraintsBuilder<TEntity> where TEntity : class
{
	private readonly EntityTypeBuilder<TEntity> _builder;

	private readonly string _columnName;
	private readonly string _tableName;

	internal DateTimeConstraintsBuilder(
		EntityTypeBuilder<TEntity> builder,
		PropertyInfo propertyInfo) {
		var isDataTypeMatch = propertyInfo.PropertyType == typeof(DateTime) ||
		                      propertyInfo.PropertyType == typeof(DateTime?);
		if (!isDataTypeMatch) throw new ArgumentException("Property type is not datetime. PropertyName: " + propertyInfo.Name, nameof(propertyInfo));

		_builder = builder;
		_tableName = _builder.Metadata.GetTableName() ?? typeof(TEntity).Name;
		_columnName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
	}

	public DateTimeConstraintsBuilder<TEntity> DateInBetween(DateTime min, DateTime max) {
		return DateInBetween(_builder.CreateUniqueConstraintName(_columnName, nameof(DateInBetween)), min, max);
	}

	public DateTimeConstraintsBuilder<TEntity> DateInBetween(string uniqueConstraintName, DateTime min, DateTime max) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= '{min:yyyy-MM-dd}' AND [{_columnName}] <= '{max:yyyy-MM-dd}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> DateMin(DateTime min) {
		return DateMin(_builder.CreateUniqueConstraintName(_columnName, nameof(DateMin)), min);
	}

	public DateTimeConstraintsBuilder<TEntity> DateMin(string uniqueConstraintName, DateTime min) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= '{min:yyyy-MM-dd}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> DateMax(DateTime max) {
		return DateMax(_builder.CreateUniqueConstraintName(_columnName, nameof(DateMax)), max);
	}

	public DateTimeConstraintsBuilder<TEntity> DateMax(string uniqueConstraintName, DateTime max) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <= '{max:yyyy-MM-dd}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> TimeInBetween(TimeSpan min, TimeSpan max) {
		return TimeInBetween(_builder.CreateUniqueConstraintName(_columnName, nameof(TimeInBetween)), min, max);
	}

	public DateTimeConstraintsBuilder<TEntity> TimeInBetween(string uniqueConstraintName, TimeSpan min, TimeSpan max) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= '{min:hh\\:mm\\:ss}' AND [{_columnName}] <= '{max:hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> TimeMin(TimeSpan min) {
		return TimeMin(_builder.CreateUniqueConstraintName(_columnName, nameof(TimeMin)), min);
	}

	public DateTimeConstraintsBuilder<TEntity> TimeMin(string uniqueConstraintName, TimeSpan min) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= '{min:hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> TimeMax(TimeSpan max) {
		return TimeMax(_builder.CreateUniqueConstraintName(_columnName, nameof(TimeMax)), max);
	}

	public DateTimeConstraintsBuilder<TEntity> TimeMax(string uniqueConstraintName, TimeSpan max) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <= '{max:hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> NotNull() {
		return NotNull(_builder.CreateUniqueConstraintName(_columnName, nameof(NotNull)));
	}

	public DateTimeConstraintsBuilder<TEntity> NotNull(string uniqueConstraintName) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] IS NOT NULL"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> NotEmpty() {
		return NotEmpty(_builder.CreateUniqueConstraintName(_columnName, nameof(NotEmpty)));
	}

	public DateTimeConstraintsBuilder<TEntity> NotEmpty(string uniqueConstraintName) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <> '0001-01-01'"));
		return this;
	}


	public DateTimeConstraintsBuilder<TEntity> Empty() {
		return Empty(_builder.CreateUniqueConstraintName(_columnName, nameof(Empty)));
	}

	public DateTimeConstraintsBuilder<TEntity> Empty(string uniqueConstraintName) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] = '0001-01-01'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> NotEmptyOrNull() {
		return NotEmptyOrNull(_builder.CreateUniqueConstraintName(_columnName, nameof(NotEmptyOrNull)));
	}

	public DateTimeConstraintsBuilder<TEntity> NotEmptyOrNull(string uniqueConstraintName) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"([{_columnName}] IS NOT NULL AND [{_columnName}] <> '0001-01-01') OR [{_columnName}] IS NULL"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> EmptyOrNull() {
		return EmptyOrNull(_builder.CreateUniqueConstraintName(_columnName, nameof(EmptyOrNull)));
	}

	public DateTimeConstraintsBuilder<TEntity> EmptyOrNull(string uniqueConstraintName) {
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"([{_columnName}] IS NULL OR [{_columnName}] = '0001-01-01')"));
		return this;
	}


	public DateTimeConstraintsBuilder<TEntity> EqualsDate(DateOnly value) {
		return EqualsDate(_builder.CreateUniqueConstraintName(_columnName, nameof(EqualsDate)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> EqualsDate(string uniqueConstraintName, DateOnly value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] = '{value:yyyy-MM-dd}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> NotEqualsDate(DateOnly value) {
		return NotEqualsDate(_builder.CreateUniqueConstraintName(_columnName, nameof(NotEqualsDate)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> NotEqualsDate(string uniqueConstraintName, DateOnly value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <> '{value:yyyy-MM-dd}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanDate(DateOnly value) {
		return GreaterThanDate(_builder.CreateUniqueConstraintName(_columnName, nameof(GreaterThanDate)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanDate(string uniqueConstraintName, DateOnly value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] > '{value:yyyy-MM-dd}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualsDate(DateOnly value) {
		return GreaterThanOrEqualDate(_builder.CreateUniqueConstraintName(_columnName, nameof(GreaterThanOrEqualDate)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDate(string uniqueConstraintName, DateOnly value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= '{value:yyyy-MM-dd}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanDate(DateOnly value) {
		return LessThanDate(_builder.CreateUniqueConstraintName(_columnName, nameof(LessThanDate)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanDate(string uniqueConstraintName, DateOnly value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] < '{value:yyyy-MM-dd}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanOrEquals(DateOnly value) {
		return LessThanOrEqual(_builder.CreateUniqueConstraintName(_columnName, nameof(LessThanOrEqual)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanOrEqual(string uniqueConstraintName, DateOnly value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <= '{value:yyyy-MM-dd}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> EqualsTime(TimeSpan value) {
		return EqualsTime(_builder.CreateUniqueConstraintName(_columnName, nameof(EqualsTime)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> EqualsTime(string uniqueConstraintName, TimeSpan value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"CAST([{_columnName}] AS TIME) = '{value:hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> NotEqualsTime(TimeSpan value) {
		return NotEqualsTime(_builder.CreateUniqueConstraintName(_columnName, nameof(NotEqualsTime)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> NotEqualsTime(string uniqueConstraintName, TimeSpan value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"CAST([{_columnName}] AS TIME) <> '{value:hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanTime(TimeSpan value) {
		return GreaterThanTime(_builder.CreateUniqueConstraintName(_columnName, nameof(GreaterThanTime)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanTime(string uniqueConstraintName, TimeSpan value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"CAST([{_columnName}] AS TIME) > '{value:hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualTime(TimeSpan value) {
		return GreaterThanOrEqualTime(_builder.CreateUniqueConstraintName(_columnName, nameof(GreaterThanOrEqualTime)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualTime(string uniqueConstraintName, TimeSpan value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"CAST([{_columnName}] AS TIME) >= '{value:hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanTime(TimeSpan value) {
		return LessThanTime(_builder.CreateUniqueConstraintName(_columnName, nameof(LessThanTime)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanTime(string uniqueConstraintName, TimeSpan value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"CAST([{_columnName}] AS TIME) < '{value:hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanOrEqualTime(TimeSpan value) {
		return LessThanOrEqualTime(_builder.CreateUniqueConstraintName(_columnName, nameof(LessThanOrEqualTime)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanOrEqualTime(string uniqueConstraintName, TimeSpan value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"CAST([{_columnName}] AS TIME) <= '{value:hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> EqualsDateTime(DateTime value) {
		return EqualsDateTime(_builder.CreateUniqueConstraintName(_columnName, nameof(EqualsDateTime)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> EqualsDateTime(string uniqueConstraintName, DateTime value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"CAST([{_columnName}] AS DATETIME) = '{value:yyyy-MM-dd hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> NotEqualsDateTime(DateTime value) {
		return NotEqualsDateTime(_builder.CreateUniqueConstraintName(_columnName, nameof(NotEqualsDateTime)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> NotEqualsDateTime(string uniqueConstraintName, DateTime value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"CAST([{_columnName}] AS DATETIME) <> '{value:yyyy-MM-dd hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanDateTime(DateTime value) {
		return GreaterThanDateTime(_builder.CreateUniqueConstraintName(_columnName, nameof(GreaterThanDateTime)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanDateTime(string uniqueConstraintName, DateTime value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"CAST([{_columnName}] AS DATETIME) > '{value:yyyy-MM-dd hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDateTime(DateTime value) {
		return GreaterThanOrEqualDateTime(_builder.CreateUniqueConstraintName(_columnName, nameof(GreaterThanOrEqualDateTime)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDateTime(string uniqueConstraintName, DateTime value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"CAST([{_columnName}] AS DATETIME) >= '{value:yyyy-MM-dd hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanDateTime(DateTime value) {
		return LessThanDateTime(_builder.CreateUniqueConstraintName(_columnName, nameof(LessThanDateTime)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanDateTime(string uniqueConstraintName, DateTime value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"CAST([{_columnName}] AS DATETIME) < '{value:yyyy-MM-dd hh\\:mm\\:ss}'"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanOrEqualDateTime(DateTime value) {
		return LessThanOrEqualDateTime(_builder.CreateUniqueConstraintName(_columnName, nameof(LessThanOrEqualDateTime)), value);
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanOrEqualDateTime(string uniqueConstraintName, DateTime value) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"CAST([{_columnName}] AS DATETIME) <= '{value:yyyy-MM-dd hh\\:mm\\:ss}'"));
		return this;
	}


	public DateTimeConstraintsBuilder<TEntity> EqualsDateTimeProperty(Expression<Func<TEntity, DateTime>> propertySelector) {
		return EqualsDateTimeProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(EqualsDateTimeProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> EqualsDateTimeProperty(string constraintName, Expression<Func<TEntity, DateTime>> propertySelector) {
		if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] = [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> NotEqualsDateTimeProperty(Expression<Func<TEntity, DateTime>> propertySelector) {
		return NotEqualsDateTimeProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(NotEqualsDateTimeProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> NotEqualsDateTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, DateTime>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <> [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanDateTimeProperty(Expression<Func<TEntity, DateTime>> propertySelector) {
		return GreaterThanDateTimeProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(GreaterThanDateTimeProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanDateTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, DateTime>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] > [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDateTimeProperty(Expression<Func<TEntity, DateTime>> propertySelector) {
		return GreaterThanOrEqualDateTimeProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(GreaterThanOrEqualDateTimeProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDateTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, DateTime>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanDateTimeProperty(Expression<Func<TEntity, DateTime>> propertySelector) {
		return LessThanDateTimeProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(LessThanDateTimeProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanDateTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, DateTime>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] < [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanOrEqualDateTimeProperty(Expression<Func<TEntity, DateTime>> propertySelector) {
		return LessThanOrEqualDateTimeProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(LessThanOrEqualDateTimeProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanOrEqualDateTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, DateTime>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <= [{_tableName}].[{compareColName}]"));
		return this;
	}


	public DateTimeConstraintsBuilder<TEntity> EqualsDateProperty(Expression<Func<TEntity, DateOnly>> propertySelector) {
		return EqualsDateProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(EqualsDateProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> EqualsDateProperty(string constraintName, Expression<Func<TEntity, DateOnly>> propertySelector) {
		if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(constraintName, $"[{_columnName}] = [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> NotEqualsDateProperty(Expression<Func<TEntity, DateOnly>> propertySelector) {
		return NotEqualsDateProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(NotEqualsDateProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> NotEqualsDateProperty(string uniqueConstraintName, Expression<Func<TEntity, DateOnly>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] <> [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanDateProperty(Expression<Func<TEntity, DateOnly>> propertySelector) {
		return GreaterThanDateProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(GreaterThanDateProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanDateProperty(string uniqueConstraintName, Expression<Func<TEntity, DateOnly>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] > [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDateProperty(Expression<Func<TEntity, DateOnly>> propertySelector) {
		return GreaterThanOrEqualDateProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(GreaterThanOrEqualDateProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDateProperty(string uniqueConstraintName, Expression<Func<TEntity, DateOnly>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName, $"[{_columnName}] >= [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanDateProperty(Expression<Func<TEntity, DateOnly>> propertySelector) {
		return LessThanDateProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(LessThanDateProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanDateProperty(string uniqueConstraintName, Expression<Func<TEntity, DateOnly>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName,
		                                           $"[{_columnName}] < [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanOrEqualDateProperty(Expression<Func<TEntity, DateOnly>> propertySelector) {
		return LessThanOrEqualDateProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(LessThanOrEqualDateProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanOrEqualDateProperty(string uniqueConstraintName, Expression<Func<TEntity, DateOnly>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName,
		                                           $"[{_columnName}] <= [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> EqualsTimeProperty(Expression<Func<TEntity, TimeSpan>> propertySelector) {
		return EqualsTimeProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(EqualsTimeProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> EqualsTimeProperty(string constraintName, Expression<Func<TEntity, TimeSpan>> propertySelector) {
		if (string.IsNullOrEmpty(constraintName)) throw new ArgumentNullException(nameof(constraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName = _builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(constraintName,
		                                           $"CAST([{_columnName}] AS TIME) = [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> NotEqualsTimeProperty(Expression<Func<TEntity, TimeSpan>> propertySelector) {
		return NotEqualsTimeProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(NotEqualsTimeProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> NotEqualsTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, TimeSpan>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName =
			_builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName,
		                                           $"CAST([{_columnName}] AS TIME) <> [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanTimeProperty(Expression<Func<TEntity, TimeSpan>> propertySelector) {
		return GreaterThanTimeProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(GreaterThanTimeProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, TimeSpan>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName =
			_builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName,
		                                           $"CAST([{_columnName}] AS TIME) > [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualTimeProperty(Expression<Func<TEntity, TimeSpan>> propertySelector) {
		return GreaterThanOrEqualTimeProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(GreaterThanOrEqualTimeProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, TimeSpan>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName =
			_builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName,
		                                           $"CAST([{_columnName}] AS TIME) >= [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanTimeProperty(Expression<Func<TEntity, TimeSpan>> propertySelector) {
		return LessThanTimeProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(LessThanTimeProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, TimeSpan>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName =
			_builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName,
		                                           $"CAST([{_columnName}] AS TIME) < [{_tableName}].[{compareColName}]"));
		return this;
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanOrEqualTimeProperty(Expression<Func<TEntity, TimeSpan>> propertySelector) {
		return LessThanOrEqualTimeProperty(_builder.CreateUniqueConstraintName(_columnName, nameof(LessThanOrEqualTimeProperty)), propertySelector);
	}

	public DateTimeConstraintsBuilder<TEntity> LessThanOrEqualTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, TimeSpan>> propertySelector) {
		if (string.IsNullOrEmpty(uniqueConstraintName)) throw new ArgumentNullException(nameof(uniqueConstraintName));
		var propertyInfo = propertySelector.GetPropertyAccess();
		var compareColName =
			_builder.Metadata.GetProperty(propertyInfo.Name).GetColumnName();
		_builder.ToTable(x => x.HasCheckConstraint(uniqueConstraintName,
		                                           $"CAST([{_columnName}] AS TIME) <= [{_tableName}].[{compareColName}]"));
		return this;
	}
}