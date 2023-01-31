namespace fika_yazılım_test.Models.ViewModels;

    public class SearchModel
    {
    public string? Ara { get;set;}
    public string[]? Hobiler { get; set; }
    public string? SinifOgretmeni { get; set; }
    public int? SortByAdSonad { get; set; }
    public int? SortByBolum { get; set; }
    public int? SortBySinifOgretmen { get; set; }
    public int? page { get; set; }
}

