using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
namespace asp_dotnet_core_webapi_template.Models;

public class YourProjectContext : DbContext
{
  public YourProjectContext(){}
  public YourProjectContext(DbContextOptions<YourProjectContext> options) : base(options) { }
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer(
        @"Server=(localdb)\mssqllocaldb;Database=YourProjectDB;Trusted_Connection=True");
  }

  public DbSet<User>? Users { get; set; }
}
