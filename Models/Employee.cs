using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrack.Models
{
  public class Employee
  {
    [Key]
    public int Id {get; set;}
    [Required(ErrorMessage = "Please enter first name.")]
    public string FirstName {get; set;}
    [Required(ErrorMessage = "Please enter last name.")]
    public string LastName {get; set;}
    public string? PhoneNumber {get; set;}
    public virtual ICollection<DocumentDetail>? DocumentDetails {get; set;}
  }
}