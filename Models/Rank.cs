using System.ComponentModel.DataAnnotations;

namespace Doctrack.Models
{
  public class Rank
  {
    [Key]
    public int Id {get; set;}
    [Required(ErrorMessage = "Please enter name title.")]
    public string Title {get; set;}
    public virtual ICollection<Employee>? Employees {get; set;}
  }
}