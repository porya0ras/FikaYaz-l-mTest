using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fika_yazılım_test.Controllers;

public class HomeController : Controller
{
    private readonly ILogger _logger;
    private readonly FikaTestDbContext _context;
    public HomeController(ILogger<HomeController> logger,
        FikaTestDbContext context)
    {
        _logger=logger;
        _context=context;
    }

    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [Route($"/{nameof(GetOgrenciler)}")]
    public async Task<IActionResult> GetOgrenciler(
             string? Ara,
             string[]? Hobiler,
             string? SinifOgretmeni,
             int? SortByAdSonad,
             int? SortByBolum,
             int? SortBySinifOgretmen,
             int? page)
    {
        if (_context.Ogrenciler == null)
        {
            return NotFound();
        }

        var result = _context.Ogrenciler.Where(e => true);

        //filtering
        if (!string.IsNullOrEmpty(Ara))
        {
            result=result.Where(e => e.NameFamily.Contains(Ara));
        }

        if (Hobiler!=null && Hobiler.Length>0)
        {
            var _hobiler = Hobiler.Select(e => Guid.Parse(e)).ToList();
            result=result.Where(e => e.Hobilar.Any(h => _hobiler.Any(h2 => h2==h.Id)));
        }

        if (!string.IsNullOrEmpty(SinifOgretmeni))
        {
            result=result.Where(e => e.SinifOgretmeni.NameFamily.Contains(SinifOgretmeni));
        }


        var finalResult = result.Select(e => new
        {
            e.Id,
            e.NameFamily,
            e.delFlag,
            e.DeleteRason,
            Hobilar = e.Hobilar.Select(h => h.Name),
            Bolum = e.Bolum.Name,
            SinifOgretmeni = e.SinifOgretmeni.NameFamily,
            RehberOgretmeni = e.RehberOgretmeni.NameFamily
        });

        //sorting
        if (SortByAdSonad!=0)
        {
            if (SortByAdSonad==1)
            {
                result=result.OrderBy(e => e.NameFamily);
            }
            else if (SortByAdSonad==-1)
            {
                result=result.OrderByDescending(e => e.NameFamily);
            }
        }

        if (SortByBolum!=0)
        {
            if (SortByBolum==1)
            {
                result=result.OrderBy(e => e.Bolum);
            }
            else if (SortByBolum==-1)
            {
                result=result.OrderByDescending(e => e.Bolum);
            }
        }

        if (SortBySinifOgretmen!=0)
        {
            if (SortBySinifOgretmen==1)
            {
                result=result.OrderBy(e => e.SinifOgretmeni);
            }
            else if (SortBySinifOgretmen==-1)
            {
                result=result.OrderByDescending(e => e.SinifOgretmeni);
            }
        }

        int pagesize = 10;
        int pageNumber = 1;
        if (page.HasValue && page.Value>0)
        {
            pageNumber=page.Value;
        }

        if (SortBySinifOgretmen==0 && SortByBolum==0 && SortByAdSonad==0)
        {
            finalResult=finalResult.OrderBy(e => e.Id);
        }

        var searchRes = await finalResult
        .Skip((pageNumber-1)*pagesize)
        .Take(pagesize)
        .ToListAsync();

        return Ok(searchRes);
    }

    [Route($"/{nameof(GetHobiler)}")]
    public async Task<IActionResult> GetHobiler()
    {
        if (_context.Hobilar == null)
        {
            return Ok(new List<int>());
        }

        var result = await _context.Hobilar.Select(e => new { e.Id, Value = e.Name }).ToListAsync();
        return Ok(result);
    }

    [Route($"/{nameof(Getbolumler)}")]
    public async Task<IActionResult> Getbolumler()
    {
        if (_context.Bolumler == null)
        {
            return Ok(new List<int>());
        }

        var result = await _context.Bolumler.Select(e => new { e.Id, Value = e.Name }).ToListAsync();
        return Ok(result);
    }

    [Route($"/{nameof(GetSinifOgretmenler)}")]
    public async Task<IActionResult> GetSinifOgretmenler()
    {
        if (_context.SinifOgretmeniler == null)
        {
            return Ok(new List<int>());
        }

        var result = await _context.SinifOgretmeniler.Select(e => new { e.Id, Value = e.NameFamily }).ToListAsync();
        return Ok(result);
    }

    [Route($"/{nameof(GetRehberOgretmenler)}")]
    public async Task<IActionResult> GetRehberOgretmenler()
    {
        if (_context.RehberOgretmeniler == null)
        {
            return Ok(new List<int>());
        }

        var result = await _context.RehberOgretmeniler.Select(e => new { e.Id, Value = e.NameFamily }).ToListAsync();
        return Ok(result);
    }


}

