namespace fika_yazılım_test.Models;

public class OgrenciEntity
{
    public Guid Id { get; set; }
    public string NameFamily { get; set; }

    public string? DeleteRason { get; set; }

    public bool delFlag { get; set; }

    public RehberOgretmenEntity RehberOgretmeni { get; set; }

    public SinifOgretmenEntity SinifOgretmeni { get; set; }

    public List<HobiEntity> Hobilar { get; set; }

    public BolumEntity Bolum { get; set; }

}

