using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Domain.Entities
{
    [Table("user")]
    public class User
    {
        [Key]
        public long Id { get; set; }

        public string Email { get; set; }
    }
}
