using efcoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace efcoreApp.Controllers
{
    public class KursKayitController : Controller
    {
        private readonly DataContext _Context;
        public KursKayitController(DataContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var kursKayitlari = await _Context.KursKayitlari.Include(x => x.Ogrenci).Include(x => x.Kurs).ToListAsync();
            return View(kursKayitlari);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Ogrenciler = new SelectList(await _Context.Ogrenciler.ToListAsync(), "OgrenciId", "AdSoyad");
            ViewBag.Kurslar = new SelectList(await _Context.Kurslar.ToListAsync(), "KursId", "KursBaslik");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KursKayit model)
        {
            model.KayitTarihi = DateTime.Now;
            _Context.KursKayitlari.Add(model);
            await _Context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
