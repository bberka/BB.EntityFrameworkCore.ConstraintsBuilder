# EfCore.ConstraintsBuilder

Simple constrains builder library for EntityFrameworkCore with Fluent API

Currently only Sql Server is supported.

Check sample project for more details.


###  Install package with NuGet Package Manager
```bash
Install-Package EfCore.ConstraintsBuilder
```

### Install package with CLI
```bash
dotnet add package EfCore.ConstraintsBuilder
```

### This project licensed under [MIT](https://choosealicense.com/licenses/mit/) license

### Usage with Fluent API

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
                .StringMinLength("UserEntity_Name_MinLength", 5);

    modelBuilder.Entity<UserEntity>()
                .AddConstraintsFor(x => x.Age)
                .NumberMin("UserEntity_Age_Min", 18)
                .NumberMax("UserEntity_Age_Max", 99);

    modelBuilder.Entity<UserEntity>()
                .AddConstraintsFor(x => x.LastName)
                .StringMinLength("UserEntity_LastName_MinLength", 10);

    modelBuilder.Entity<UserEntity>()
                .AddConstraintsFor(x => x.AccountValidFor)
                .NumberInBetween("UserEntity_AccountValidFor_Between", 1, 30);
  }
}
```


### Usage with Data Annotations

#### Applying Data Annotations to User Entity
```csharp
public class UserEntity
{
  [StringLength(10, MinimumLength = 5)]
  [RegularExpression("^[a-zA-Z0-9]*$")]
  public string Name { get; set; }
  
  [Range(18, 99)]
  public int Age { get; set; }
  
  [StringLength(10, MinimumLength = 10)]
  public string LastName { get; set; }
  
  [Range(1, 30)]
  public int AccountValidFor { get; set; }
}
```

#### Applying Constraints to User Entity inside DbContext
```csharp
public class SampleDbContext : DbContext
{
  
  public DbSet<UserEntity> Users { get; set; }
  
  protected override void OnModelCreating(ModelBuilder modelBuilder) {
    modelBuilder.Entity<UserEntity>()
                .AddConstraintsFromDataAnnotations();
  }
}
```
