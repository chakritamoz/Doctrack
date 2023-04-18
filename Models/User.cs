
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrack.Models
{
  public class User
  {
    public int Id { get; set; }
    [Required]
    public string Username { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    [Required]
    public string Email { get; set; }
    public string Role_Id { get; set; }
    [ForeignKey("Role_Id")]
    public virtual Role Role { get; set; }
    public bool IsEmailConfirm { get; set; } = false;
    public bool IsApproved { get; set; } = false;
  }
}