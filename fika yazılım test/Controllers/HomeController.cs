using fika_yazılım_test.Models.ViewModels;
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
    public async Task<IActionResult> GetOgrenciler(SearchModel val)
    {
        if (_context.Ogrenciler == null)
        {
            return Ok(null);
        }

        var result = _context.Ogrenciler.Where(e => true);

        //filtering
        if (!string.IsNullOrEmpty(val.Ara))
        {
            result=result.Where(e => e.NameFamily.Contains(val.Ara));
        }

        if (val.Hobiler!=null && val.Hobiler.Length>0)
        {
            var _hobiler = val.Hobiler.Select(e => Guid.Parse(e)).ToList();
            result=result.Where(e => e.Hobilar.Any(h => _hobiler.Any(h2 => h2==h.Id)));
        }

        if (!string.IsNullOrEmpty(val.SinifOgretmeni))
        {
            var _SinifOgretmeniId = Guid.Parse(val.SinifOgretmeni);
            result=result.Where(e => e.SinifOgretmeni.Id.Equals(_SinifOgretmeniId));
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
        if (val.SortByAdSonad.HasValue && val.SortByAdSonad!=0)
        {
            if (val.SortByAdSonad==1)
            {
                finalResult=finalResult.OrderBy(e => e.NameFamily);
            }
            else if (val.SortByAdSonad==-1)
            {
                finalResult=finalResult.OrderByDescending(e => e.NameFamily);
            }
        }

        if (val.SortByBolum.HasValue && val.SortByBolum!=0)
        {
            if (val.SortByBolum==1)
            {
                finalResult=finalResult.OrderBy(e => e.Bolum);
            }
            else if (val.SortByBolum==-1)
            {
                result=result.OrderByDescending(e => e.Bolum);
            }
        }

        if (val.SortBySinifOgretmen.HasValue && val.SortBySinifOgretmen!=0)
        {
            if (val.SortBySinifOgretmen==1)
            {
                finalResult=finalResult.OrderBy(e => e.SinifOgretmeni);
            }
            else if (val.SortBySinifOgretmen==-1)
            {
                finalResult=finalResult.OrderByDescending(e => e.SinifOgretmeni);
            }
        }

        int pagesize = 10;
        int pageNumber = 1;
        if (val.page.HasValue && val.page.Value>0)
        {
            pageNumber=val.page.Value;
        }

        if (val.SortBySinifOgretmen==0 && val.SortByBolum==0 && val.SortByAdSonad==0)
        {
            finalResult=finalResult.OrderBy(e => e.Id);
        }

        var count = await finalResult.CountAsync();

        var searchRes = await finalResult
        .Skip((pageNumber-1)*pagesize)
        .Take(pagesize)
        .ToListAsync();



        return Ok(new { searchRes, count });
    }


    [Route("/GetOgrenci/{id}")]
    public async Task<IActionResult> GetOgrenci(string id)
    {
        if (_context.Ogrenciler == null)
        {
            return Ok(null);
        }

        if (string.IsNullOrEmpty(id))
        {
            return Ok(null);
        }

        var gUId = Guid.Parse(id);

        var row = await _context.Ogrenciler.Where(e => e.Id==gUId).Select(e => new
        {
            e.Id,
            e.NameFamily,
            Bolum = new { e.Bolum.Id, label = e.Bolum.Name },
            Hobilar = e.Hobilar.Select(e => new { e.Id, label = e.Name }),
            SinifOgretmeni = new { e.SinifOgretmeni.Id, label = e.SinifOgretmeni.NameFamily },
            RehberOgretmeni = new { e.RehberOgretmeni.Id, label = e.RehberOgretmeni.NameFamily }
        }).FirstOrDefaultAsync();

        return Ok(row);
    }


    [Route($"/{nameof(GetHobiler)}")]
    public async Task<IActionResult> GetHobiler()
    {
        if (_context.Hobilar == null)
        {
            return Ok(new List<int>());
        }

        var result = await _context.Hobilar.Select(e => new { e.Id, label = e.Name }).ToListAsync();
        return Ok(result);
    }

    [Route($"/{nameof(Getbolumler)}")]
    public async Task<IActionResult> Getbolumler()
    {
        if (_context.Bolumler == null)
        {
            return Ok(new List<int>());
        }

        var result = await _context.Bolumler.Select(e => new { e.Id, label = e.Name }).ToListAsync();
        return Ok(result);
    }

    [Route($"/{nameof(GetSinifOgretmenler)}")]
    public async Task<IActionResult> GetSinifOgretmenler()
    {
        if (_context.SinifOgretmeniler == null)
        {
            return Ok(new List<int>());
        }

        var result = await _context.SinifOgretmeniler.Select(e => new { e.Id, label = e.NameFamily }).ToListAsync();
        return Ok(result);
    }

    [Route($"/{nameof(GetRehberOgretmenler)}")]
    public async Task<IActionResult> GetRehberOgretmenler()
    {
        if (_context.RehberOgretmeniler == null)
        {
            return Ok(new List<int>());
        }

        var result = await _context.RehberOgretmeniler.Select(e => new { e.Id, label = e.NameFamily }).ToListAsync();
        return Ok(result);
    }

    [HttpPost]
    [Route($"/{nameof(Sil)}")]
    public async Task<IActionResult> Sil(DeleteModel val)
    {
        if (_context.Ogrenciler == null)
        {
            throw new Exception("NotFound");
        }
        var gUId = Guid.Parse(val.Id);
        var row =await _context.Ogrenciler.Where(e => e.Id==gUId).FirstOrDefaultAsync();

        if(row== null)
        {
            throw new Exception("NotFound");
        }


        if(row.delFlag==true)
        {
            return Ok(true);
        }
        else
        {
            row.delFlag=true;
            row.DeleteRason=val.Reason;
            await _context.SaveChangesAsync();

            return Ok(true);
        }
    }

    
    [HttpPost]
    [Route($"/{nameof(Kaydet)}")]
    public async Task<IActionResult> Kaydet(EditModel val)
    {
        if (_context.Ogrenciler == null)
        {
            throw new Exception("NotFound");
        }
        var gUId = Guid.Parse(val.Id);
        var row =await _context.Ogrenciler.Where(e => e.Id==gUId).FirstOrDefaultAsync();

        if(row== null)
        {
            throw new Exception("NotFound");
        }


        if(row.delFlag==true)
        {
            return Ok(true);
        }
        else
        {
            row.delFlag=true;
            row.DeleteRason=val.Reason;
            await _context.SaveChangesAsync();

            return Ok(true);
        }
    }
}

