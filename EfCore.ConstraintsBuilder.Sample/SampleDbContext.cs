using Microsoft.EntityFrameworkCore;

namespace EfCore.ConstraintsBuilder.Sample;

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