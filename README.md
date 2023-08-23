# EfCore.ConstraintsBuilder

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


### Supported Database Providers
- [x] Sql Server

### Warning
This library is in early development stage, use it at your own risk.

If you are planning to reference constraints from anywhere else it is highly recommended to specify constraint name explicitly.

Auto generated constraint names may change in future versions.

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
Example: `User_Name_Regex_UnqiueGuid`



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
```