﻿namespace $safeprojectname$.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<TExample> TExample { get; set; }

    }
}
