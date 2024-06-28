using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class CuisineDTO
    {
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string CuisineName { get; set; }
        public CuisineDTO()
        {
            CuisineName = "";
        }
    }
}