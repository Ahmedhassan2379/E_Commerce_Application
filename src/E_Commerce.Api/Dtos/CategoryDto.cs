using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Api.Dtos
{
    public class CategoryDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
