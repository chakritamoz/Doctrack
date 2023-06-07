using System.ComponentModel.DataAnnotations;

namespace Doctrack.Models
{
  public class Rank
  {
    [Key]
    public int Id {get; set;}
    [Required(ErrorMessage = "Please enter title name.")]
    public string Title {get; set;}
    public virtual ICollection<JobRankDetail>? JobRankDetails {get; set;}
    public virtual ICollection<DocumentDetail>? DocumentDetails {get; set;}
  }
}