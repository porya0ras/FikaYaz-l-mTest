using fika_yazılım_test.Models;
using Microsoft.EntityFrameworkCore;

namespace fika_yazılım_test;

public class FikaTestDbContext : DbContext
{
    public FikaTestDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<ÖğrenciEntity> Öğrencilar { get; set; }
    public DbSet<BölümEntity> Bölümlar { get; set; }
    public DbSet<HobiEntity> Hobilar { get; set; }
    public DbSet<SinifÖgretmenEntity> SinifÖgretmenilar { get; set; }
    public DbSet<RehberÖgretmenEntity> RehberÖgretmenilar { get; set; }
}

