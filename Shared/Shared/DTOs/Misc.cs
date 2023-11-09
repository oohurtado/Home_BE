using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public class PersonDTO
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }
    }

    public class DummyDTO
    {
        public string? STR { get; set; }
    }
}