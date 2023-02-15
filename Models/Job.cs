using System.ComponentModel.DataAnnotations;

namespace Doctrack.Models
{
  public class Job
  {
    [Key]
    public int Id {get; set;}
    [Required(ErrorMessage = "Please enter job title.")]
    public string Title {get; set;}
    public virtual ICollection<Employee>? Employees {get; set;}
  }
}