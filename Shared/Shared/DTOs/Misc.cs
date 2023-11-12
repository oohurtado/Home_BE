using System.ComponentModel.DataAnnotations;

// for testings
namespace Shared.DTOs
{
    public class TestDTO
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

    public class CardProcessed
    {
        public string? Card { get; set; }
        public bool? Rejected { get; set; }
    }
        
}