using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Models
{
    public class Student
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
