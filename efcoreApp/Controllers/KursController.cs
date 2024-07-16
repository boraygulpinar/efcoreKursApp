using efcoreApp.Data;
using efcoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class KursController : Controller
    {
        private readonly DataContext _dataContext;
        public KursController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var kurslar = await _dataContext.Kurslar.Include(k => k.Ogretmen).ToListAsync();
            return View(kurslar);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Ogretmenler = new SelectList(await _dataContext.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(KursViewModel model)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Kurslar.Add(new Kurs() { KursId = model.KursId, KursBaslik = model.KursBaslik, OgretmenId = model.OgretmenId });
                await _dataContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Ogretmenler = new SelectList(await _dataContext.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurs = await _dataContext
                            .Kurslar
                            .Include(k => k.KursKayitlari)
                            .ThenInclude(k => k.Ogrenci)
                            .Select(k => new KursViewModel
                            {
                                KursId = k.KursId,
                                KursBaslik = k.KursBaslik,
                                OgretmenId = k.OgretmenId,
                                KursKayitlari = k.KursKayitlari
                            })
                            .FirstOrDefaultAsync(k => k.KursId == id);

            if (kurs == null)
            {
                return NotFound();
            }

            ViewBag.Ogretmenler = new SelectList(await _dataContext.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");

            return View(kurs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KursViewModel model)
        {
            if (id != model.KursId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dataContext.Update(new Kurs() { KursId = model.KursId, KursBaslik = model.KursBaslik, OgretmenId = model.OgretmenId });
                    await _dataContext.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!_dataContext.Kurslar.Any(k => k.KursId == model.KursId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewBag.Ogretmenler = new SelectList(await _dataContext.Ogretmenler.ToListAsync(), "OgretmenId", "AdSoyad");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kurs = await _dataContext.Kurslar.FindAsync(id);

            if (kurs == null)
            {
                return NotFound();
            }

            return View(kurs);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var kurs = await _dataContext.Kurslar.FindAsync(id);
            if (kurs == null)
            {
                return NotFound();
            }
            _dataContext.Kurslar.Remove(kurs);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
