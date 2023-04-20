using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrack.Models
{
  public class Role
  {
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public virtual ICollection<Account> Accounts { get; set; }
  }
}