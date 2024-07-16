using efcoreApp.Data;
using efcoreApp.Models;
using System.ComponentModel.DataAnnotations;
namespace efcoreApp.Models
{
    public class KursViewModel
    {
        public int KursId { get; set; }
        [Required]
        [StringLength(100)]
        public string? KursBaslik { get; set; }
        public int OgretmenId { get; set; }
        public ICollection<KursKayit> KursKayitlari { get; set; } = new List<KursKayit>();

    }
}
