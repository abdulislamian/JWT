using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Models
{
    public class TokenStore
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public bool isValid { get; set; }

        //public DateTime IssueDate { get; set; }
    }
}
