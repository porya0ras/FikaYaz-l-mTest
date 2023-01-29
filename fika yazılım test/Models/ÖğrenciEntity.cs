namespace fika_yazılım_test.Models;

public class ÖğrenciEntity
{
    public Guid Id { get; set; }
    public string NameFamily { get; set; }

    public RehberÖgretmenEntity RehberÖgretmeni { get; set; }

    public SinifÖgretmenEntity SinifÖgretmeni { get; set; }

    public List<HobiEntity> Hobilar { get; set; }

    public BölümEntity Bölüm { get; set; }

}

