using System.ComponentModel.DataAnnotations;

namespace JobTrack.Api.Dtos
{
    public class JobApplicationRequest
    {
        [Required]
        [StringLength(100)]
        public string Company { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Position { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime AppliedDate { get; set; }

        [Required]
        [RegularExpression(
            "^(Applied|Interview|Offer|Rejected)$",
            ErrorMessage = "Status must be Applied, Interview, Offer or Rejected."
        )]
        public string Status { get; set; } = string.Empty;
    }
}