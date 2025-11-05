using Microsoft.EntityFrameworkCore;
using System;
using MyAzureApi.Entities;

namespace MyAzureApi.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        { }
        public DbSet<Student> Students { get; set; }
    }
}
