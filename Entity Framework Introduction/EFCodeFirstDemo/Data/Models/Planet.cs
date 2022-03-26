
using System.ComponentModel.DataAnnotations;

namespace EFCodeFirstDemo.Data.Models
{
    public class Planet
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int SunId { get; set; }

        public Star Sun { get; set; }

        public int SolarSystemId { get; set; }

        public SolarSystem SolarSystem { get; set; }


    }
}
