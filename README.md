# BB.EntityFrameworkCore.ConstraintsBuilder

Simple constrains builder library for EntityFrameworkCore with Fluent API


Check sample project for more details.

This project licensed under [MIT](https://choosealicense.com/licenses/mit/) license

##  Install package

Nuget Package Manager
```bash
Install-Package EfCore.ConstraintsBuilder
```

CLI
```bash
dotnet add package EfCore.ConstraintsBuilder
```

### Supported Data Types 
- [x] String
- [x] Int (Int32)
- [x] Long (Int64)
- [x] Short (Int16)
- [x] Byte (Int8)
- [x] DateTime
- [x] Guid


### Supported Database Providers
- [x] Sql Server

### Warnings and Disclaimer
This library is in early development stage not all functions is tested, use it at your own risk.

If you are planning to reference constraints from anywhere else it is highly recommended to specify constraint name explicitly.

Auto generated constraint names may change in future versions.


### What is this library for?

This library is for adding constraints to your entities with fluent api. 

EntityFrameworkCore allows to add constraints however it is easy to make mistakes because you have to write plain SQL and it is hard to read. 

By using this library you are avoiding possible errors and making your code more readable.

### Why not use DataAnnotations or FluentValidation instead ?

DataAnnotations and FluentValidation are great libraries but they are in different layer than EntityFrameworkCore.

Constraints purpose is to be applied directly into database. Even if your application layer is validating data, it is still possible to insert invalid data to database.
This library is there for you to avoid that.

If a constraint rule is not matched database will throw an exception and your application will not insert invalid data to database.
It is recommended to catch that exception explicitly and handle it properly.

If you are trying to completely avoid exceptions, you should consider using DataAnnotations or FluentValidation or any other validation library.

### How about performance ?

This library only initialized once on migration creation. It is not affecting performance of your application directly. 

However since library adds constraints to database it may affect performance of your database of Insert or Update operations.
Avoid using complex constraints and stick with simple ones.

If performance of your database is a major concern for you, you should consider using this library with caution or not use it at all.

### Tips
- Avoid using complex constraints
- Avoid using constraints on nullable properties, all tho it is possible to use constraints on nullable properties, it is not recommended. My cause unexpected errors.
- Avoid using constraints on properties that is used in queries frequently to improve performance.
- Instead of string types use enum types when you can and validate data on application layer or let EntityFrameworkCore to handle it for you by using EnumToStringConverter or EnumToNumberConverter.

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

### IntConstraintsBuilder Methods
```csharp
IntConstraintsBuilder<TEntity> NumberInBetween(int min, int max);
IntConstraintsBuilder<TEntity> NumberInBetween(string constraintName, int min, int max);
IntConstraintsBuilder<TEntity> NumberMin(int min);
IntConstraintsBuilder<TEntity> NumberMin(string constraintName, int min);
IntConstraintsBuilder<TEntity> NumberMax(int max);
IntConstraintsBuilder<TEntity> NumberMax(string constraintName, int max);
IntConstraintsBuilder<TEntity> EqualOneOf(IEnumerable<int> acceptedValues);
IntConstraintsBuilder<TEntity> EqualOneOf(string constraintName, IEnumerable<int> acceptedValues);
```

### LongConstraintsBuilder Methods
```csharp
LongConstraintsBuilder<TEntity> NumberInBetween(long min, long max);
LongConstraintsBuilder<TEntity> NumberInBetween(string uniqueConstraintName, long min, long max);
LongConstraintsBuilder<TEntity> NumberMin(long min);
LongConstraintsBuilder<TEntity> NumberMin(string uniqueConstraintName, long min);
LongConstraintsBuilder<TEntity> NumberMax(long max);
LongConstraintsBuilder<TEntity> NumberMax(string uniqueConstraintName, long max);
LongConstraintsBuilder<TEntity> EqualOneOf(IEnumerable<long> acceptedValues);
LongConstraintsBuilder<TEntity> EqualOneOf(string uniqueConstraintName, IEnumerable<long> acceptedValues);
```


### ShortConstraintsBuilder Methods
```csharp
ShortConstraintsBuilder<TEntity> NumberInBetween(short min, short max);
ShortConstraintsBuilder<TEntity> NumberInBetween(string uniqueConstraintName, short min, short max);
ShortConstraintsBuilder<TEntity> NumberMin(short min);
ShortConstraintsBuilder<TEntity> NumberMin(string uniqueConstraintName, short min);
ShortConstraintsBuilder<TEntity> NumberMax(short max);
ShortConstraintsBuilder<TEntity> NumberMax(string uniqueConstraintName, short max);
ShortConstraintsBuilder<TEntity> EqualOneOf(IEnumerable<short> acceptedValues);
ShortConstraintsBuilder<TEntity> EqualOneOf(string uniqueConstraintName, IEnumerable<short> acceptedValues);
```


### ByteConstraintsBuilder Methods
```csharp
ByteConstraintsBuilder<TEntity> NumberInBetween(byte min, byte max);
ByteConstraintsBuilder<TEntity> NumberInBetween(string uniqueConstraintName, byte min, byte max);
ByteConstraintsBuilder<TEntity> NumberMin(byte min);
ByteConstraintsBuilder<TEntity> NumberMin(string uniqueConstraintName, byte min);
ByteConstraintsBuilder<TEntity> NumberMax(byte max);
ByteConstraintsBuilder<TEntity> NumberMax(string uniqueConstraintName, byte max);
ByteConstraintsBuilder<TEntity> EqualOneOf(IEnumerable<byte> acceptedValues);
ByteConstraintsBuilder<TEntity> EqualOneOf(string uniqueConstraintName, IEnumerable<byte> acceptedValues);
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
