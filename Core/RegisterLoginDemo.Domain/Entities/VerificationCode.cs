using System.ComponentModel.DataAnnotations;

namespace RegisterLoginDemo.Domain.Entities
{
    public class VerificationCode
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "The code must be a 6-digit number.")]
        public string? Code { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public SendType Type { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ExpirationTime { get; set; }

        [Required]
        public bool IsVerified { get; set; }
    }
    public enum SendType
    {
        SMS,
        Email,
    }

}