using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace webapi.Models;

public class YourProjectContext : DbContext
{
    public YourProjectContext() { }

    public YourProjectContext(DbContextOptions<YourProjectContext> options)
        : base(options) { }
        
    public DbSet<User> Users => Set<User>();
}
