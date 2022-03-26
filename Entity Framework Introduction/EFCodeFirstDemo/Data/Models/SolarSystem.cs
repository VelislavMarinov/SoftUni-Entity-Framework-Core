
using System.ComponentModel.DataAnnotations;

namespace EFCodeFirstDemo.Data.Models
{
    public class SolarSystem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }


    }
}
