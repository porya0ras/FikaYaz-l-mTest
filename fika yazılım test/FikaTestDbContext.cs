using fika_yazılım_test.Models;
using Microsoft.EntityFrameworkCore;

namespace fika_yazılım_test;

public class FikaTestDbContext : DbContext
{
    public FikaTestDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<OgrenciEntity> Ogrenciler { get; set; }
    public DbSet<BolumEntity> Bolumler { get; set; }
    public DbSet<HobiEntity> Hobilar { get; set; }
    public DbSet<SinifOgretmenEntity> SinifOgretmeniler { get; set; }
    public DbSet<RehberOgretmenEntity> RehberOgretmeniler { get; set; }
}

