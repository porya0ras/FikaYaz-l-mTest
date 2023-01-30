using fika_yazılım_test.Models;
namespace fika_yazılım_test.Data;


public static class SeedData
{


    public static async Task InitialAsync(IServiceProvider services)
    {
        await AddTestData(services.GetRequiredService<FikaTestDbContext>());
    }

    public static async Task AddTestData(
        FikaTestDbContext context
    )
    {
        if (!context.Ogrenciler.Any())
        {

            var list = new HobiEntity[]{new HobiEntity()
            {
                Id=Guid.NewGuid(),
                Name="Yuzme"
            }, new HobiEntity()
            {
                Id=Guid.NewGuid(),
                Name="Basketbal"
            },new HobiEntity()
            {
                Id=Guid.NewGuid(),
                Name="Musik"
            },new HobiEntity()
            {
                Id=Guid.NewGuid(),
                Name="Edebiyat"
            }};

            context.Hobilar.AddRange(list.ToList());
            context.SaveChanges();

            var list2 = new RehberOgretmenEntity[]{new RehberOgretmenEntity()
            {
                Id=Guid.NewGuid(),
                NameFamily="Melda Sonmez"
            }, new RehberOgretmenEntity()
            {
                Id=Guid.NewGuid(),
                NameFamily="Nida Akyaz"
            }};

            context.RehberOgretmeniler.AddRange(list2.ToList());
            context.SaveChanges();

            var list3 = new SinifOgretmenEntity[]{new SinifOgretmenEntity()
            {
                Id=Guid.NewGuid(),
                NameFamily="Namik  Son"
            }, new SinifOgretmenEntity()
            {
                Id=Guid.NewGuid(),
                NameFamily="Halil Mese"
            }};

            context.SinifOgretmeniler.AddRange(list3.ToList());
            context.SaveChanges();


            var list4 = new BolumEntity[]{new BolumEntity()
            {
                Id=Guid.NewGuid(),
                Name="Felsefe"
            }, new BolumEntity()
            {
                Id=Guid.NewGuid(),
                Name="Matematial"
            }};

            context.Bolumler.AddRange(list4.ToList());
            context.SaveChanges();


            var list5 = new OgrenciEntity[]{new OgrenciEntity()
            {
                Id=Guid.NewGuid(),
                NameFamily="Mehmet Soylemez",
                Bolum=list4[0],
                Hobilar=new List<HobiEntity> {list[0],list[1] },
                RehberOgretmeni=list2[0],
                SinifOgretmeni=list3[0]
            }, new OgrenciEntity()
            {
                Id=Guid.NewGuid(),
                NameFamily="Berkay Hinci",
                 Bolum=list4[1],
                Hobilar=new List<HobiEntity> {list[2],list[3] },
                RehberOgretmeni=list2[1],
                SinifOgretmeni=list3[1]
            }};

            context.Ogrenciler.AddRange(list5.ToList());
            context.SaveChanges();
        }
    }
}
