using System.ComponentModel.DataAnnotations;

namespace SampleWebApiAspNetCore.Dtos
{
    public class GenCreateDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Vision { get; set; }
        public int Rarity { get; set; }
        public DateTime Created { get; set; }
    }
}
