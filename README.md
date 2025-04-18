# BB.EntityFrameworkCore.ConstraintsBuilder

A simple library for applying database constraints with Entity Framework Core's Fluent API.

**Note:** This library currently targets and is primarily tested with **SQL Server**.

[![NuGet Version](https://img.shields.io/nuget/v/BB.EntityFrameworkCore.ConstraintsBuilder.SqlServer?style=for-the-badge)](https://www.nuget.org/packages/BB.EntityFrameworkCore.ConstraintsBuilder.SqlServer/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://github.com/bberka/BB.EntityFrameworkCore.ConstraintsBuilder/blob/master/LICENSE.txt)

This project is licensed under the [MIT License](https://choosealicense.com/licenses/mit/).

## Installation

Install the NuGet package `BB.EntityFrameworkCore.ConstraintsBuilder.SqlServer` into your Entity Framework Core project.

**NuGet Package Manager Console:**

```bash
Install-Package BB.EntityFrameworkCore.ConstraintsBuilder.SqlServer
```

**.NET CLI:**

```bash
dotnet add package BB.EntityFrameworkCore.ConstraintsBuilder.SqlServer
```

## Features

### Supported Database Providers

* ✅ SQL Server

### Supported Data Types

The library aims to provide constraint building capabilities for fundamental data types:

* ✅ String
* ✅ Numeric Types (including `int`, `double`, `decimal`, etc.)
* ✅ DateTime
* ✅ Guid

## Why Use This Library?

Entity Framework Core provides the ability to add database constraints using the Fluent API, but it often requires
writing raw SQL strings. This can be error-prone and reduces code readability.

`BB.EntityFrameworkCore.ConstraintsBuilder` simplifies this process by providing a more strongly-typed and fluent way to
define common constraints like `CHECK` constraints directly within your `DbContext`'s `OnModelCreating` method.

By using this library, you can:

* **Reduce Errors:** Avoid syntax errors common in raw SQL constraints.
* **Improve Readability:** Define constraints using C# syntax that is often easier to understand and maintain.
* **Enforce Data Integrity at the Database Level:** Ensure that invalid data cannot be inserted or updated in your
  database, even if application-level validation is bypassed.

## How it Differs from Validation Libraries (DataAnnotations, FluentValidation)

Validation libraries like DataAnnotations and FluentValidation operate at the **application layer**. They are excellent
for providing user feedback and preventing invalid data from reaching your data access layer.

Database constraints, on the other hand, operate at the **database layer**. They are the final line of defense for data
integrity. If data attempts to be inserted or updated through any means (your application, direct SQL scripts, other
applications), the database will enforce the constraint.

* **Application Validation:** Prevents bad data from being *sent* to the database.
* **Database Constraints:** Prevents bad data from being *saved* in the database.

It is highly recommended to use both application-level validation and database constraints for robust data integrity.

When a database constraint is violated, your application will typically receive an exception. It's crucial to handle
these exceptions gracefully in your application code.

## Performance Considerations

This library's impact on performance is primarily during **migration creation** or **database initialization**, as it
defines the constraints in the database schema. The library itself does not introduce runtime overhead during typical
application operations (reading or writing data), other than adding the constraint definitions.

However, the **constraints themselves** added to the database can affect the performance of `INSERT` and `UPDATE`
operations. The database has to evaluate the constraint for every affected row.

* **Simple Constraints:** Constraints like checking if a number is positive or a string has a minimum length usually
  have a minimal performance impact.
* **Complex Constraints:** Constraints involving complex logic, multiple columns, or functions might introduce
  noticeable overhead.

If database write performance is a critical concern, use complex constraints with caution or consider alternative data
integrity strategies.

## Usage Tips

* **Specify Constraint Names:** For easier debugging and migration management, it is highly recommended to explicitly
  define constraint names when using the library, especially if you might need to reference them later. Auto-generated
  names can change in future versions.
* **Avoid Complex Constraints:** Stick to simple and easily verifiable conditions within your constraints.
* **Nullable Properties:** While the library may allow adding constraints to nullable properties, exercise caution. The
  behavior of `CHECK` constraints with `NULL` values can sometimes be counter-intuitive (a `NULL` value often passes a
  `CHECK` constraint unless specifically handled, e.g., `WHERE columnName IS NOT NULL AND columnName > 0`). Explicitly
  defining the behavior for `NULL` is important if you constrain nullable columns.
* **Frequently Queried Columns:** Adding constraints to columns heavily used in `WHERE` clauses of frequently executed
  queries might slightly impact query plan generation, although the effect is often negligible for simple constraints.
  Focus on correctness first and optimize if profiling reveals issues.
* **Leverage Enum Converters:** For properties representing a fixed set of values, consider using
  `EnumToStringConverter` or `EnumToNumberConverter` with EF Core. This leverages the type system for validation and
  allows EF Core to handle the underlying database representation, often eliminating the need for manual constraints on
  those specific columns.

## Early Development Warning

This library is currently in an early stage of development. While efforts are made to test functionality, not all
scenarios may be fully covered or optimized.

* **Use with Caution:** Evaluate its suitability for your project's needs.
* **Report Issues:** If you encounter any bugs or unexpected behavior, please report them on the GitHub repository.
* **Contributions Welcome:** Contributions are welcome to help improve the library!

---

## Usage with Fluent API

### Basic User Entity

```csharp
public class UserEntity
{
  public string Name { get; set; }
  public string Id { get; set; }
  public string LastName { get; set; }
  public int Age { get; set; }
  public int AccountValidFor { get; set; }
}
```

### Applying Constraints to User Entity inside DbContext

```csharp
public class SampleDbContext : DbContext
{
  
  public DbSet<UserEntity> Users { get; set; }
  
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<UserEntity>()
                .AddConstraintsFor(x => x.Name)
                .RegexExpression("UserEntity_Name_Regex", "^[a-zA-Z0-9]*$")
                .MinLength(1)
                .MaxLength(25);

    modelBuilder.Entity<UserEntity>()
                .AddConstraintsFor(x => x.Age)
                .NumberMin("UserEntity_Age_Min", 18)
                .NumberMax("UserEntity_Age_Max", 99);

    modelBuilder.Entity<UserEntity>()
                .AddConstraintsFor(x => x.LastName)
                .MinLength("UserEntity_LastName_MinLength", 10);

    modelBuilder.Entity<UserEntity>()
                .AddConstraintsFor(x => x.AccountValidFor)
                .NumberInBetween("UserEntity_AccountValidFor_Between", 1, 30);    
  }
}
```

## Builders and methods

Currently supported builder methods listed below, more will be added in future.

If no constraint name provided for a method, a unique constraint name will be generated.

Format: `%TableName%_%ColumnName%_%ConstraintFunctionName%_%ConstraintIndex%`

Example: `UserEntity_Name_Regex_1`

### StringConstraintsBuilder Methods

```csharp
StringConstraintsBuilder<TEntity> EmailAddress();
StringConstraintsBuilder<TEntity> EmailAddress(string constraintName);
StringConstraintsBuilder<TEntity> Url();
StringConstraintsBuilder<TEntity> Url(string constraintName);
StringConstraintsBuilder<TEntity> PhoneNumber();
StringConstraintsBuilder<TEntity> PhoneNumber(string constraintName);
StringConstraintsBuilder<TEntity> RegexExpression(string regex);
StringConstraintsBuilder<TEntity> RegexExpression(string constraintName, string regex);
StringConstraintsBuilder<TEntity> MinLength(int minLength);
StringConstraintsBuilder<TEntity> MinLength(string constraintName, int minLength);
StringConstraintsBuilder<TEntity> MaxLength(int maxLength);
StringConstraintsBuilder<TEntity> MaxLength(string constraintName, int maxLength);
StringConstraintsBuilder<TEntity> LengthBetween(int minLength, int maxLength);
StringConstraintsBuilder<TEntity> LengthBetween(string constraintName, int minLength, int maxLength);
StringConstraintsBuilder<TEntity> EqualsOneOf(IEnumerable<string> acceptedValues);
StringConstraintsBuilder<TEntity> EqualsOneOf(string constraintName, IEnumerable<string> acceptedValues);
StringConstraintsBuilder<TEntity> NotEqualsOneOf(IEnumerable<string> acceptedValues);
StringConstraintsBuilder<TEntity> NotEqualsOneOf(string constraintName, IEnumerable<string> acceptedValues);
StringConstraintsBuilder<TEntity> Equals(string value);
StringConstraintsBuilder<TEntity> Equals(string constraintName, string value);
StringConstraintsBuilder<TEntity> NotEquals(string value);
StringConstraintsBuilder<TEntity> NotEquals(string constraintName, string value);
StringConstraintsBuilder<TEntity> StartsWith(string value);
StringConstraintsBuilder<TEntity> StartsWith(string constraintName, string value);
StringConstraintsBuilder<TEntity> EndsWith(string value);
StringConstraintsBuilder<TEntity> EndsWith(string constraintName, string value);
StringConstraintsBuilder<TEntity> Contains(string value);
StringConstraintsBuilder<TEntity> Contains(string constraintName, string value);
StringConstraintsBuilder<TEntity> NotContains(string value);
StringConstraintsBuilder<TEntity> NotContains(string constraintName, string value);
StringConstraintsBuilder<TEntity> Empty();
StringConstraintsBuilder<TEntity> Empty(string constraintName);
StringConstraintsBuilder<TEntity> NotEmpty();
StringConstraintsBuilder<TEntity> NotEmpty(string constraintName);
StringConstraintsBuilder<TEntity> NullOrWhiteSpace();
StringConstraintsBuilder<TEntity> NullOrWhiteSpace(string constraintName);
StringConstraintsBuilder<TEntity> NotNullOrWhiteSpace();
StringConstraintsBuilder<TEntity> NotNullOrWhiteSpace(string constraintName);
StringConstraintsBuilder<TEntity> EqualProperty(Expression<Func<TEntity, string>> propertySelector);
StringConstraintsBuilder<TEntity> EqualProperty(string constraintName, Expression<Func<TEntity, string>> propertySelector);
StringConstraintsBuilder<TEntity> NotEqualProperty(Expression<Func<TEntity, string>> propertySelector);
StringConstraintsBuilder<TEntity> NotEqualProperty(string constraintName, Expression<Func<TEntity, string>> propertySelector);
```

### NumberConstraintsBuilder Methods

```csharp
ByteConstraintsBuilder<TEntity, TProperty> NumberInBetween(TProperty min, TProperty max);
ByteConstraintsBuilder<TEntity, TProperty> NumberInBetween(string uniqueConstraintName, TProperty min, TProperty max);
ByteConstraintsBuilder<TEntity, TProperty> NumberMin(TProperty min);
ByteConstraintsBuilder<TEntity, TProperty> NumberMin(string uniqueConstraintName, TProperty min);
ByteConstraintsBuilder<TEntity, TProperty> NumberMax(TProperty max);
ByteConstraintsBuilder<TEntity, TProperty> NumberMax(string uniqueConstraintName, TProperty max);
ByteConstraintsBuilder<TEntity, TProperty> EqualOneOf(IEnumerable<TProperty> acceptedValues);
ByteConstraintsBuilder<TEntity, TProperty> EqualOneOf(string uniqueConstraintName, IEnumerable<TProperty> acceptedValues);
```

### DateTimeConstraintsBuilder Methods

```csharp
DateTimeConstraintsBuilder<TEntity> DateInBetween(DateTime min, DateTime max);
DateTimeConstraintsBuilder<TEntity> DateInBetween(string uniqueConstraintName, DateTime min, DateTime max);
DateTimeConstraintsBuilder<TEntity> DateMin(DateTime min);
DateTimeConstraintsBuilder<TEntity> DateMin(string uniqueConstraintName, DateTime min);
DateTimeConstraintsBuilder<TEntity> DateMax(DateTime max);
DateTimeConstraintsBuilder<TEntity> DateMax(string uniqueConstraintName, DateTime max);
DateTimeConstraintsBuilder<TEntity> TimeInBetween(TimeSpan min, TimeSpan max);
DateTimeConstraintsBuilder<TEntity> TimeInBetween(string uniqueConstraintName, TimeSpan min, TimeSpan max);
DateTimeConstraintsBuilder<TEntity> TimeMin(TimeSpan min);
DateTimeConstraintsBuilder<TEntity> TimeMin(string uniqueConstraintName, TimeSpan min);
DateTimeConstraintsBuilder<TEntity> TimeMax(TimeSpan max);
DateTimeConstraintsBuilder<TEntity> TimeMax(string uniqueConstraintName, TimeSpan max);
DateTimeConstraintsBuilder<TEntity> NotNull();
DateTimeConstraintsBuilder<TEntity> NotNull(string uniqueConstraintName);
DateTimeConstraintsBuilder<TEntity> NotEmpty();
DateTimeConstraintsBuilder<TEntity> NotEmpty(string uniqueConstraintName);
DateTimeConstraintsBuilder<TEntity> Empty();
DateTimeConstraintsBuilder<TEntity> Empty(string uniqueConstraintName);
DateTimeConstraintsBuilder<TEntity> NotEmptyOrNull();
DateTimeConstraintsBuilder<TEntity> NotEmptyOrNull(string uniqueConstraintName);
DateTimeConstraintsBuilder<TEntity> EmptyOrNull();
DateTimeConstraintsBuilder<TEntity> EmptyOrNull(string uniqueConstraintName);
DateTimeConstraintsBuilder<TEntity> EqualsDate(DateOnly value);
DateTimeConstraintsBuilder<TEntity> EqualsDate(string uniqueConstraintName, DateOnly value);
DateTimeConstraintsBuilder<TEntity> NotEqualsDate(DateOnly value);
DateTimeConstraintsBuilder<TEntity> NotEqualsDate(string uniqueConstraintName, DateOnly value);
DateTimeConstraintsBuilder<TEntity> GreaterThanDate(DateOnly value);
DateTimeConstraintsBuilder<TEntity> GreaterThanDate(string uniqueConstraintName, DateOnly value);
DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualsDate(DateOnly value);
DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDate(string uniqueConstraintName, DateOnly value);
DateTimeConstraintsBuilder<TEntity> LessThanDate(DateOnly value);
DateTimeConstraintsBuilder<TEntity> LessThanDate(string uniqueConstraintName, DateOnly value);
DateTimeConstraintsBuilder<TEntity> LessThanOrEquals(DateOnly value);
DateTimeConstraintsBuilder<TEntity> LessThanOrEqual(string uniqueConstraintName, DateOnly value);
DateTimeConstraintsBuilder<TEntity> EqualsTime(TimeSpan value);
DateTimeConstraintsBuilder<TEntity> EqualsTime(string uniqueConstraintName, TimeSpan value);
DateTimeConstraintsBuilder<TEntity> NotEqualsTime(TimeSpan value);
DateTimeConstraintsBuilder<TEntity> NotEqualsTime(string uniqueConstraintName, TimeSpan value);
DateTimeConstraintsBuilder<TEntity> GreaterThanTime(TimeSpan value);
DateTimeConstraintsBuilder<TEntity> GreaterThanTime(string uniqueConstraintName, TimeSpan value);
DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualTime(TimeSpan value);
DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualTime(string uniqueConstraintName, TimeSpan value);
DateTimeConstraintsBuilder<TEntity> LessThanTime(TimeSpan value);
DateTimeConstraintsBuilder<TEntity> LessThanTime(string uniqueConstraintName, TimeSpan value);
DateTimeConstraintsBuilder<TEntity> LessThanOrEqualTime(TimeSpan value);
DateTimeConstraintsBuilder<TEntity> LessThanOrEqualTime(string uniqueConstraintName, TimeSpan value);
DateTimeConstraintsBuilder<TEntity> EqualsDateTime(DateTime value);
DateTimeConstraintsBuilder<TEntity> EqualsDateTime(string uniqueConstraintName, DateTime value);
DateTimeConstraintsBuilder<TEntity> NotEqualsDateTime(DateTime value);
DateTimeConstraintsBuilder<TEntity> NotEqualsDateTime(string uniqueConstraintName, DateTime value);
DateTimeConstraintsBuilder<TEntity> GreaterThanDateTime(DateTime value);
DateTimeConstraintsBuilder<TEntity> GreaterThanDateTime(string uniqueConstraintName, DateTime value);
DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDateTime(DateTime value);
DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDateTime(string uniqueConstraintName, DateTime value);
DateTimeConstraintsBuilder<TEntity> LessThanDateTime(DateTime value);
DateTimeConstraintsBuilder<TEntity> LessThanDateTime(string uniqueConstraintName, DateTime value);
DateTimeConstraintsBuilder<TEntity> LessThanOrEqualDateTime(DateTime value);
DateTimeConstraintsBuilder<TEntity> LessThanOrEqualDateTime(string uniqueConstraintName, DateTime value);
DateTimeConstraintsBuilder<TEntity> EqualsDateTimeProperty(Expression<Func<TEntity, DateTime>> propertySelector);
DateTimeConstraintsBuilder<TEntity> EqualsDateTimeProperty(string constraintName, Expression<Func<TEntity, DateTime>> propertySelector);
DateTimeConstraintsBuilder<TEntity> NotEqualsDateTimeProperty(Expression<Func<TEntity, DateTime>> propertySelector);
DateTimeConstraintsBuilder<TEntity> NotEqualsDateTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, DateTime>> propertySelector);
DateTimeConstraintsBuilder<TEntity> GreaterThanDateTimeProperty(Expression<Func<TEntity, DateTime>> propertySelector);
DateTimeConstraintsBuilder<TEntity> GreaterThanDateTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, DateTime>> propertySelector);
DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDateTimeProperty(Expression<Func<TEntity, DateTime>> propertySelector);
DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDateTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, DateTime>> propertySelector);
DateTimeConstraintsBuilder<TEntity> LessThanDateTimeProperty(Expression<Func<TEntity, DateTime>> propertySelector);
DateTimeConstraintsBuilder<TEntity> LessThanDateTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, DateTime>> propertySelector);
DateTimeConstraintsBuilder<TEntity> LessThanOrEqualDateTimeProperty(Expression<Func<TEntity, DateTime>> propertySelector);
DateTimeConstraintsBuilder<TEntity> LessThanOrEqualDateTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, DateTime>> propertySelector);
DateTimeConstraintsBuilder<TEntity> EqualsDateProperty(Expression<Func<TEntity, DateOnly>> propertySelector);
DateTimeConstraintsBuilder<TEntity> EqualsDateProperty(string constraintName, Expression<Func<TEntity, DateOnly>> propertySelector);
DateTimeConstraintsBuilder<TEntity> NotEqualsDateProperty(Expression<Func<TEntity, DateOnly>> propertySelector);
DateTimeConstraintsBuilder<TEntity> NotEqualsDateProperty(string uniqueConstraintName, Expression<Func<TEntity, DateOnly>> propertySelector);
DateTimeConstraintsBuilder<TEntity> GreaterThanDateProperty(Expression<Func<TEntity, DateOnly>> propertySelector);
DateTimeConstraintsBuilder<TEntity> GreaterThanDateProperty(string uniqueConstraintName, Expression<Func<TEntity, DateOnly>> propertySelector);
DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDateProperty(Expression<Func<TEntity, DateOnly>> propertySelector);
DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualDateProperty(string uniqueConstraintName, Expression<Func<TEntity, DateOnly>> propertySelector);
DateTimeConstraintsBuilder<TEntity> LessThanDateProperty(Expression<Func<TEntity, DateOnly>> propertySelector);
DateTimeConstraintsBuilder<TEntity> LessThanDateProperty(string uniqueConstraintName, Expression<Func<TEntity, DateOnly>> propertySelector);
DateTimeConstraintsBuilder<TEntity> LessThanOrEqualDateProperty(Expression<Func<TEntity, DateOnly>> propertySelector);
DateTimeConstraintsBuilder<TEntity> LessThanOrEqualDateProperty(string uniqueConstraintName, Expression<Func<TEntity, DateOnly>> propertySelector);
DateTimeConstraintsBuilder<TEntity> EqualsTimeProperty(Expression<Func<TEntity, TimeSpan>> propertySelector);
DateTimeConstraintsBuilder<TEntity> EqualsTimeProperty(string constraintName, Expression<Func<TEntity, TimeSpan>> propertySelector);
DateTimeConstraintsBuilder<TEntity> NotEqualsTimeProperty(Expression<Func<TEntity, TimeSpan>> propertySelector);
DateTimeConstraintsBuilder<TEntity> NotEqualsTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, TimeSpan>> propertySelector);
DateTimeConstraintsBuilder<TEntity> GreaterThanTimeProperty(Expression<Func<TEntity, TimeSpan>> propertySelector);
DateTimeConstraintsBuilder<TEntity> GreaterThanTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, TimeSpan>> propertySelector);
DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualTimeProperty(Expression<Func<TEntity, TimeSpan>> propertySelector);
DateTimeConstraintsBuilder<TEntity> GreaterThanOrEqualTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, TimeSpan>> propertySelector);
DateTimeConstraintsBuilder<TEntity> LessThanTimeProperty(Expression<Func<TEntity, TimeSpan>> propertySelector);
DateTimeConstraintsBuilder<TEntity> LessThanTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, TimeSpan>> propertySelector);
DateTimeConstraintsBuilder<TEntity> LessThanOrEqualTimeProperty(Expression<Func<TEntity, TimeSpan>> propertySelector);
DateTimeConstraintsBuilder<TEntity> LessThanOrEqualTimeProperty(string uniqueConstraintName, Expression<Func<TEntity, TimeSpan>> propertySelector);
```

### GuidConstraintsBuilder Methods

```csharp
GuidConstraintsBuilder<TEntity> NotNull();
GuidConstraintsBuilder<TEntity> NotNull(string uniqueConstraintName);
GuidConstraintsBuilder<TEntity> NotEmpty();
GuidConstraintsBuilder<TEntity> NotEmpty(string uniqueConstraintName);
GuidConstraintsBuilder<TEntity> Empty();
GuidConstraintsBuilder<TEntity> Empty(string uniqueConstraintName);
GuidConstraintsBuilder<TEntity> NotEmptyOrNull();
GuidConstraintsBuilder<TEntity> NotEmptyOrNull(string uniqueConstraintName);
GuidConstraintsBuilder<TEntity> EmptyOrNull();
GuidConstraintsBuilder<TEntity> EmptyOrNull(string uniqueConstraintName);
GuidConstraintsBuilder<TEntity> EqualsProperty(Expression<Func<TEntity, Guid>> propertySelector);
GuidConstraintsBuilder<TEntity> EqualsProperty(string constraintName, Expression<Func<TEntity, Guid>> propertySelector);
GuidConstraintsBuilder<TEntity> NotEqualsProperty(Expression<Func<TEntity, Guid>> propertySelector);
GuidConstraintsBuilder<TEntity> NotEqualsProperty(string uniqueConstraintName, Expression<Func<TEntity, Guid>> propertySelector);
GuidConstraintsBuilder<TEntity> EqualsValue(Guid value);
GuidConstraintsBuilder<TEntity> EqualsValue(string uniqueConstraintName, Guid value);
GuidConstraintsBuilder<TEntity> NotEqualsValue(Guid value);
GuidConstraintsBuilder<TEntity> NotEqualsValue(string uniqueConstraintName, Guid value);
GuidConstraintsBuilder<TEntity> EqualsOneOf(IEnumerable<Guid> values);
GuidConstraintsBuilder<TEntity> EqualsOneOf(string uniqueConstraintName, IEnumerable<Guid> values);
GuidConstraintsBuilder<TEntity> NotEqualsOneOf(IEnumerable<Guid> values);
GuidConstraintsBuilder<TEntity> NotEqualsOneOf(string uniqueConstraintName, IEnumerable<Guid> values);
```
