using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doctrack.Models
{
  public class Employee
  {
    [Key]
    public int Id {get; set;}
    [Required(ErrorMessage = "Please enter employee title.")]
    public int Rank_Id {get; set;}
    [ForeignKey("Rank_Id")]
    public virtual Rank Rank {get; set;}
    [Required(ErrorMessage = "Please enter job title.")]
    public int Job_Id {get; set;}
    [ForeignKey("Job_Id")]
    public virtual Job Job {get; set;}
    [Required(ErrorMessage = "Please enter first name.")]
    public string FirstName {get; set;}
    [Required(ErrorMessage = "Please enter last name.")]
    public string LastName {get; set;}
    public string? PhoneNumber {get; set;}
  }
}