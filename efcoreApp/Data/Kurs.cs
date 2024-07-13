using System.ComponentModel.DataAnnotations;

namespace efcoreApp.Data
{
    public class Kurs
    {
        [Key]
        public int KursId { get; set; }
        public string? KursBaslik { get; set; }
    }
}
